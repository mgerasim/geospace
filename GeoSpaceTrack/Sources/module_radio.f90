 module radio
 implicit none
 private

 public getM3000_foF2

 contains
 
! Расчет M3000, foF2 по заданному месяцу и числу Вольфа. 
! Линейная интерполяция между значениями числа Вольфа 0 и 100, больше - экстраполяция.
! Выполнена по данным ионосферной модели IRI2007: 
! ftp://nssdcftp.gsfc.nasa.gov/models/ionospheric/iri/iri2007/
! там же находятся исходные поля для восстновления CCIRXX.asc и URSIXX.asc
! Исходные подпрограммы FOUT и GAMMA1 из файла iri_sub.for
!
! Опциональные параметры запуска:
! CCIR = .true. => модель CCIR, иначе - модель URSI
! CCIR - для континентов, URSI - для океана. Для глобуса брать URSI.
! РОМАНСКИЙ, 05.10.2015
! ВАЖНО: см. комментарий где интерполяция
 subroutine getM3000_foF2(modip, lat, lon, month, hour, W, M3000, foF2, DATADIR, CCIR)
 real, intent(in) :: modip             ! геомагнитная широта
 real, intent(in) :: lat               ! географическая широта (от -90 до +90 = Сев. полюс)
 real, intent(in) :: lon               ! географическая долгота (от 0 до 360)
 integer, intent(in) :: month          ! месяц
 integer, intent(in) :: hour           ! час суток (ВСВ)
 integer, intent(in) :: W              ! число Вольфа на месяц month 
 character(*), intent(in) :: DATADIR   ! путь к файлам с данными модели CCIR и URSI 
 logical, intent(in), optional :: CCIR ! тип модели
 real, intent(out) :: M3000, foF2      ! коэф. M3000 безразм. и макс. крит. частота слоя F2 в МГц.
 logical :: flag, is_used
 ! коэффициенты разложения
 real :: F2(13,76,2), FM3(9,49,2)
 ! коэф. разложения, проинтерполированные для данного числа Вольфа
 real :: FF0_W(998), FM3_W(441), coef
 ! массивы, хранящие наибольшую широту для каждой гармоники по долготе    
 integer, dimension(9), parameter :: QF = (/11,11,8,4,1,0,0,0,0/)
 integer, dimension(7), parameter :: QM = (/6,7,5,2,1,0,0/)
 character(len = 255) :: dir
 character(len = 100) :: fl
 integer :: un, jn, jm, k

 flag = .true.
 if(present(CCIR)) flag = CCIR
 dir = DATADIR
 if(dir(len_trim(dir):len_trim(dir)) /= '\')dir = trim(dir)//'\'

! print '("DEBUG ", A)', dir
!print *, "DEBUG", modip, lat, lon, month
!print *, "DEBUG", hour, W, M3000, foF2
!print *, "DEBUG", DATADIR, CCIR
 
! чтение файлов модели 
 do un = 112, 998
    inquire(unit = un, opened = is_used)
    if(.not. is_used) exit
 enddo
! при любом раскладе нужны файлы ccir, т.к. из них читаются коэф. M3000
 fl = 'ccirXX.asc'
 write(fl(5:6),'(i2)') month + 10
 open(un, file = trim(dir)//fl, &
  status = 'old', &
  form = 'formatted')
 read(un,'(1x,4E15.8)')F2,FM3
 close(un)
! данные URSI, если заказано
 if (.not. flag) then
    fl = 'ursiXX.asc'
    write(fl(5:6),'(i2)') month + 10
    open(un, file = trim(dir)//fl, &
     status = 'old', &
     form = 'formatted')
    read(un,'(1x,4E15.8)')F2
    close(un)
 endif

! интерполяция для заданного числа Вольфа
! номер 3ьего индекса 1 - для W=0, 2 - для W=100 
!!!!! по хорошему интерполяцию нужно делать по номеру дня месяца и вообще
!!!!! не использовать число Вольфа: 
!!!!! см. в iri_sub.for и iri_fun.for подпрограммы tcon и moda
 coef = W / 100.0 ! коэффициент интерполяции или экстраполяции
! foF2
 do jn = 1, 76
 do jm = 1, 13
     k = (jn - 1) * 13 + jm     
     FF0_W(k) = (1.0 - coef) * F2(jm,jn,1) + coef * F2(jm,jn,2)
 enddo
 enddo
 ! коэф. M3000 
 do jn = 1, 49
 do jm = 1, 9
    k = (jn - 1) * 9 + jm
    FM3_W(k) = (1.0 - coef) * FM3(jm,jn,1) + coef * FM3(jm,jn,2)   
 enddo
 enddo
 ! восстановление M3000, foF2 в заданной точке для данного часа
 M3000 = GAMMA1(modip,lat,lon,hour,4,QM,7,49,9,441,FM3_W)
 foF2  = GAMMA1(modip,lat,lon,hour,6,QF,9,76,13,988,FF0_W)

!print *, "DEBUG", M3000, foF2

 end subroutine getM3000_foF2

 ! Восстановления полей M3000 и foF2 в точке
 ! ftp://nssdcftp.gsfc.nasa.gov/models/ionospheric/iri/iri2007/irifun.for
 ! M=1+NQ1+2(NQ2+1)+2(NQ3+1)+... .                  
 ! SHEIKH,4.3.77. 
 real function GAMMA1(SMODIP,SLAT,SLONG,HOUR,IHARM,NQ,K1,M,MM,M3,SFE)
 real, intent(in) :: SMODIP, SLAT, SLONG
 integer, intent(in) :: HOUR ! UTC
 integer, intent(in) :: IHARM
 integer, dimension(IHARM), intent(in) :: NQ
 integer, intent(in) :: K1, M, MM, M3
 real, dimension(M3), intent(in) :: SFE
 
 real*8 :: C(12), S(12), COEF(100), SUM 
 real, dimension(13) :: XSINX
 real :: HOU, PI, SS, S3, S0, S1, S2
 integer :: I, J, MI, INDEX, NP, L
 
 PI = ATAN(1.0) * 4.0
 HOU = (15.0 * HOUR - 180.0) * PI / 180.0
 S(1) = SIN(HOU)
 C(1) = COS(HOU)
 
 do I = 2, IHARM
    C(I) = C(1) * C(I-1) - S(1) * S(I-1)                 
    S(I) = C(1) * S(I-1) + S(1) * C(I-1)
 enddo
 
 do I = 1, M
    MI = (I-1) * MM
    COEF(I) = SFE(MI+1)
    do J = 1, IHARM
	COEF(I) = COEF(I) + SFE(MI+2*J) * S(J) + SFE(MI+2*J+1) * C(J) 
    enddo
 enddo
 
 SUM=COEF(1)     
 SS=SIN(SMODIP * PI / 180.0)                           
 S3=SS           
 XSINX(1)=1.0    
 INDEX=NQ(1)     
 do J = 1, INDEX                             
    SUM=SUM+COEF(1+J)*SS                         
    XSINX(J+1)=SS   
    SS=SS*S3        
 enddo        

 XSINX(NQ(1)+2)=SS                            
 NP=NQ(1)+1      
 SS=COS(SLAT * PI / 180.0)                             
 S3=SS           

 do J=2,K1   
    S0=SLONG * (J - 1.) * PI / 180.0                        
    S1=COS(S0)      
    S2=SIN(S0)      
    INDEX=NQ(J)+1   
    do L=1,INDEX                             
	NP=NP+1         
        SUM=SUM+COEF(NP)*XSINX(L)*SS*S1              
        NP=NP+1         
        SUM=SUM+COEF(NP)*XSINX(L)*SS*S2              
    enddo        
    SS=SS*S3        
 enddo       
  
 GAMMA1=SUM  
 end function GAMMA1 

 end module radio

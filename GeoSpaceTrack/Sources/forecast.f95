module forecast

use const
use error
use calculation

implicit none

public forecast_MUF
public calc_layer_E_F1_F2_for_MUF_OPF
public calc_layerF2_for_MUF_OPF

contains

subroutine forecast_MUF( W, month, KTO, XO, YO, D )
	integer                  :: KTO, month, W, h
	real                     :: D
	real, dimension ( SIZE ) :: XO, YO
	real, dimension ( HOURS ) :: MUF, OPF

	if( KTO == 1 ) then
		call calc_layer_E_F1_F2_for_MUF_OPF( W, month, KTO, XO, YO, D, MUF, OPF )
	else
		call calc_layerF2_for_MUF_OPF( W, month, KTO, XO, YO, D, MUF, OPF )
	end if

	print *, "DEBUG MUF:"
	do h = 1, HOURS, 1
		print *, "DEBUG", h, "-", MUF(h), "-", OPF(h)
	end do

    return
end subroutine forecast_MUF


subroutine calc_layer_E_F1_F2_for_MUF_OPF( W, month, KTO, XO, YO, D, MUF, OPF )
	integer                  :: KTO, month, W, h
	real                     :: D
	real, dimension ( SIZE ) :: XO, YO
	real, dimension ( HOURS ) :: foE, Z, MUF, OPF, MPE, MPF1, MPF2, foF1
	real :: A, B, Zm, MDE, FF, F, obj, obj1, DF, MDF1
	CHARACTER(50) error

	call rad_to_degree( XO(1), obj )
	Zm = 90 - obj + DS(month)
	DF = (0.0006 + 0.00009*Zm) * W
	call  rad_to_degree( (Zm + 27)/1.3, obj )
	A = 2.8*sin(obj) + DF
	B = (1/(-6-Zm)**2) * log( A / 0.8 )

	FF = D / (2*R)
	F = 3000 / (2*R)

	if( D < 2000 ) then
		obj = cos( atan( sin(FF) / (1.017915968 - cos(FF)) ) )
		if( obj == 0 ) then
			obj = EPS
		end if
		MDE = 1 / obj
	else
		MDE = 5.27
	end if

	if( D < 3000 ) then
		obj = cos( atan( sin(FF) / (1.03603959 - cos(FF)) ) )
		if( obj == 0 ) then
			obj = EPS
		end if
		MDF1 = 1 / obj
	else
		MDF1 = 3.8
	end if

	do h = 1, HOURS, 1
		call rad_to_degree( YO(1), obj )
		call degree_to_rad( 15*((h-1) + obj/15), obj)
		call degree_to_rad( 1.0*DS(month), obj1 )
		Z(h) = sin(XO(1)) * sin(obj1) - cos(XO(1)) * cos(obj1) * cos(obj)

		if( abs( Z(h) ) > 1 ) then
			write(error, *) Z(h)
			error = "parametr with arcsin | Z(h) | > 1" // error
			call error_func( 1, error )
		end if

		Z(h) = asin( Z(h) )
		foF1(h) = cos( (PI/2) - Z(h) )
		if( foF1(h) < 0 ) then
			foF1(h) = 0
		end if
		foF1(h) = 4.5 * (1 + 0.00241*W) * ( (foF1(h))**22 )**0.01

		call rad_to_degree( Z(h), Z(h) )
		foE(h) = 0.6 + A*exp( -B*(Z(h)-Zm)**2 )
		
		!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		MPF2(h) = 0.0
		!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	end do
	
	MPE = foE * MDE
	MPF1 = foF1 * MDf1

	do h = 1, HOURS, 1
		MUF(h) = max( MPE(h), MPF1(h), MPF2(h) )

		if( MPE(h) > MPF1(h) .and. MPE(h) > MPF2(h) ) then
			OPF(h) = MPE(h)
		end if

		if( MPF1(h) > MPE(h) .and. MPF1(h) > MPF2(h) ) then
			OPF(h) = MPF1(h) * 0.95
		end if

		if( MPF2(h) > MPE(h) .and. MPF2(h) > MPF1(h) ) then
			OPF(h) = MPF2(h) * 0.85
		end if

		if( MPE(h) == MPF1(h) .and. MPE(h) > MPF2(h) ) then
			OPF(h) = MPE(h)
		end if

		if( MPE(h) == MPF2(h) .and. MPE(h) > MPF1(h) ) then
			OPF(h) = MPE(h)
		end if

		if( MPF1(h) == MPF2(h) .and. MPF1(h) > MPE(h) ) then
			OPF(h) = MPF1(h) * 0.95
		end if
	end do

	return
end subroutine calc_layer_E_F1_F2_for_MUF_OPF

subroutine calc_layerF2_for_MUF_OPF( W, month, KTO, XO, YO, D, MUF, OPF )
	integer                  :: KTO, month, W, h
	real                     :: D
	real, dimension ( SIZE ) :: XO, YO
	real, dimension ( HOURS ) :: foE, Z, MUF, OPF
	real :: A, B, Zm, MDE, FF, F, obj, obj1, DF
	CHARACTER(50) error


	print *, "DEBUG NoNe"
end subroutine calc_layerF2_for_MUF_OPF


end module forecast






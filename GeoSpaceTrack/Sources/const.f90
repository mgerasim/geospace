module const

public

real, parameter       :: R = 6372.795               ! Средний радиус Земли
real, parameter       :: PI = 3.141592654
integer, parameter    :: SIZE = 6
real, parameter       :: EPS = 0.0001
integer, parameter    :: HOURS = 25
integer, parameter    :: MONTHS = 12
integer, dimension(MONTHS) :: DS = (/-22, -14, -4, 8, 18, 23, 22, 16, 5, -7, -17, -23/)
end module const
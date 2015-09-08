module error

public
contains
	
	subroutine error_func( number, str )
		CHARACTER(50) str

		select case (number)
			case(0)
				print *, "   ERROR: |cos| > 1 " // str
			case(1)
				print *, "   ERROR: incorrect value " // str
			case(2)
				print *, "   ERROR: |sin| > 1 " // str
		end select

		call exit(1)

		return
	end subroutine error_func 

end module error

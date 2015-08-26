module testmod

public :: smod

contains
subroutine smod()
	print *, "DEBUG Hello, World!"
end subroutine smod

end module testmod
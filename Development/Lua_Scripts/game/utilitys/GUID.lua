local GUID = class()
GUID.__id__ = 10000

function GUID:get()
	self.__id__ = self.__id__ + 1
	return self.__id__
end

return GUID
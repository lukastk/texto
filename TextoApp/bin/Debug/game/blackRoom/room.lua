blackRoom = inheritClasses(roomBase);
blackRoom.type = "room";
blackRoom.identifier = "blackRoom";
blackRoom.names = {"black room", "room"};
blackRoom.items = {};
--blackRoom.visible = true;
--Locations, used with goNorth and etc.
blackRoom.north = "";
blackRoom.south = "";
blackRoom.east = "";
blackRoom.west = "";

blackRoom.isDark = true;

function blackRoom:init()
	roomBase.init(self);
end

function blackRoom:look()
	showMessage("You are in a dark black room.");

	if self.isDark then
		showMessage("Some guy says: \"Hello? Who's there?\"");
	else
		showMessage("There is a guy with a flashlight in front of you.");
	end
end

function blackRoom:actionIndLookAt(sentence)
	self:look();
end

function blackRoom:goNorth(sentence)
	return false;
end

function blackRoom:goSouth(sentence)
	return getItem("whiteRoomDoor"):goThrough();
end

function blackRoom:goWest(sentence)
	return false;
end

function blackRoom:goEast(sentence)
	return false;
end

return blackRoom;
whiteRoom = inheritClasses(roomBase);
whiteRoom.type = "room";
whiteRoom.identifier = "whiteRoom";
whiteRoom.names = {"white room", "room"};
whiteRoom.items = {};
--whiteRoom.visible = true;
--Locations, used with goNorth and etc.
whiteRoom.north = "blackRoom";
whiteRoom.south = "";
whiteRoom.east = "";
whiteRoom.west = "";

whiteRoom.playerIsLyingDown = true;

function whiteRoom:init()
	roomBase.init(self);
end

function whiteRoom:look()
	showMessage(
		"You are in an extremely white room, as if the creator of this gam- oops i mean room, didn't even bother " ..
		"to add some imagination into it all. You are lying down on a bed, around you there is a window, a desk with some " ..
		"stuff on it and a chest.\n\n" ..
		"There is a door to the north.");
end

function whiteRoom:actionIndLookAt(sentence)
	self:look();
end

whiteRoom.literalActions.GetUp = {"get up"};
function whiteRoom:actionLitGetUp(entry)
	if (not self.playerIsLyingDown) then
		showMessage("You are already up!");
	else
		if (self.items[blanket.identifier] == nil) then
			self.playerIsLyingDown = false;
			showMessage("You got up from the bed, what a go-getter.");
		else
			showMessage("You try but the incredible force of the blanket on top of you makes it impossible to get up.");
		end
	end
end

function whiteRoom:goNorth(sentence)
	return getItem("whiteRoomDoor"):goThrough();
end

function whiteRoom:goSouth(sentence)
	return false;
end

function whiteRoom:goWest(sentence)
	return false;
end

function whiteRoom:goEast(sentence)
	return false;
end

return whiteRoom;
door = inheritClasses(doorBase);
door.type = "item";
door.identifier = "whiteRoomDoor";
door.location = "whiteRoom";
door.insideRoom = "whiteRoom";
door.names = {"door"};
door.items = {};
door.visible = true;

door.leadsTo1 = "blackRoom";
door.leadsTo2 = "whiteRoom";
door.locks["whiteRoomBronzeKey"] = true;

function door:init()
	itemBase.init(self);
end

function door:look()
	showMessage("It is a door.");
end

function door:actionIndLookAt(sentence)
	self:look();
end

return door;
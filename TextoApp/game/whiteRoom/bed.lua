bed = inheritClasses(itemBase);
bed.type = "item";
bed.identifier = "whiteRoomBed";
bed.location = "whiteRoom";
bed.insideRoom = "whiteRoom";
bed.names = {"white bed", "bed"};
bed.items = {};
bed.visible = true;

function bed:init()
	itemBase.init(self);
end

function bed:look()
	if (whiteRoom.playerIsLyingDown) then
		showMessage("It's a bed.");
	else
		showMessage("It's a white bed, with a pillow and a blanket on top. It's the one you woke up on.");
	end
end

function bed:actionIndLookAt(sentence)
	self:look();
end

return bed;
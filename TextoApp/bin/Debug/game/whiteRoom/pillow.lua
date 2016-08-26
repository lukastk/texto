pillow = inheritClasses(itemBase);
pillow.type = "item";
pillow.identifier = "whiteRoomPillow";
pillow.location = "whiteRoom";
pillow.insideRoom = "whiteRoom";
pillow.names = {"white pillow", "pillow"};
pillow.items = {};
pillow.visible = true;

function pillow:init()
	itemBase.init(self);
end

function pillow:look()
	if (whiteRoom.playerIsLyingDown) then
		showMessage("If only you could turn your head a 180 degrees...");
	else
		showMessage("It's a pillow.");
	end
end

function pillow:actionIndLookAt(sentence)
	self:look();
end

return pillow;
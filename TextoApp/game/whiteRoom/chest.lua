chest = inheritClasses(itemBase);
chest.type = "item";
chest.identifier = "whiteRoomChest";
chest.location = "whiteRoom";
chest.insideRoom = "whiteRoom";
chest.names = {"treasure chest", "chest"};
chest.items = {};
chest.visible = true;

function chest:init()
	itemBase.init(self);
end

function chest:look()
	if self.canSeeInside then
		showMessage("It's a chest containing large amounts gold.");
	else
		showMessage("It looks like one of those treasure chests in movies that contains large amounts of gold.");
	end
end

function chest:actionIndLookAt(sentence)
	self:look();
end

function chest:actionOpen(sentence)
	if whiteRoom.playerIsLyingDown then
		showMessage("It might be a good idea to get up first...");
		return;
	end
	
	if not self.canSeeInside then
		self.canSeeInside = true;
		showMessage("You opened the chest, and suprise suprise... It contains huge amounts of gold.");

		if self.items["whiteRoomBronzeKey"] ~= nil then
			showMessage("On top of all the gold you see a bronze key.");
		end
	else
		showMessage("It already is open!");
	end
end

function chest:actionClose(sentence)
	if self.canSeeInside then
		self.canSeeInside = false
		showMessage("You closed the chest.");
	else
		showMessage("It already is closed!");
	end
end

function chest:canTakeFromInventory(item)
	return true;
end

return chest;
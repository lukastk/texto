gold = inheritClasses(itemBase);
gold.type = "item";
gold.identifier = "whiteRoomGold";
gold.location = "whiteRoomChest";
gold.insideRoom = "whiteRoom";
gold.names = {"gold"};
gold.items = {};
gold.visible = true;

function gold:init()
	itemBase.init(self);
end

function gold:look()
	showMessage("Gold, worth gold.");
end

function gold:actionIndLookAt(sentence)
	self:look();
end

gold["actionTake"] = makeTakeAction(gold, "You stuffed your pockets with as much gold as you can carry.", "You can't carry any more gold!");

return gold;
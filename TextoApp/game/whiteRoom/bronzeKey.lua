bronzeKey = inheritClasses(keyBase);
bronzeKey.type = "item";
bronzeKey.identifier = "whiteRoomBronzeKey";
bronzeKey.location = "whiteRoomChest";
bronzeKey.insideRoom = "whiteRoom";
bronzeKey.names = {"bronze key", "key"};
bronzeKey.items = {};
bronzeKey.visible = true;

function bronzeKey:init()
	itemBase.init(self);
end

function bronzeKey:look()
	showMessage("It is a bronze key.");
end

function bronzeKey:actionIndLookAt(sentence)
	self:look();
end

bronzeKey["actionTake"] = makeDefaultTakeAction(bronzeKey);

return bronzeKey;
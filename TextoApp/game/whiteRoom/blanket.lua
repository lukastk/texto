blanket = inheritClasses(itemBase);
blanket.type = "item";
blanket.identifier = "whiteRoomBlanket";
blanket.location = "whiteRoom";
blanket.insideRoom = "whiteRoom";
blanket.names = {"white blanket", "blanket"};
blanket.items = {};
blanket.visible = true;

function blanket:init()
	itemBase.init(self);
end

function blanket:look()
	if (whiteRoom.playerIsLyingDown and player.items[blanket.identifier] == nil) then
		showMessage("A white blanket is covering you.");
	else
		showMessage("You see a white blanket.");
	end
end

function blanket:actionIndLookAt(sentence)
	self:look();
end

blanket["actionTake"] = makeTakeAction(blanket, "You took the blanket, which frankly made you a little bit cold.", "You already have a blanket!");

makeSynonyms(blanket, blanket.actionTake, {"actionRemove"});

return blanket;
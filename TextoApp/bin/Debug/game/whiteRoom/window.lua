window = inheritClasses(itemBase);
window.type = "item";
window.identifier = "whiteRoomWindow";
window.location = "whiteRoom";
window.insideRoom = "whiteRoom";
window.names = {"window"};
window.items = {};
window.visible = true;

function window:init()
	itemBase.init(self);
end

function window:look()
	showMessage("It's a window.");
end

function window:actionIndLookAt(sentence)
	self:look();
end

function window:actionIndLookThrough(sentence)
	showMessage("You see... a brick wall.");
	getItem("whiteRoomBrickWall").visible = true
end

return window;
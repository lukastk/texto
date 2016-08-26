brickWall = inheritClasses(itemBase);
brickWall.type = "item";
brickWall.identifier = "whiteRoomBrickWall";
brickWall.location = "whiteRoom";
brickWall.insideRoom = "whiteRoom";
brickWall.names = {"brick wall"};
brickWall.items = {};
brickWall.visible = false;

function brickWall:init()
	itemBase.init(self);
end

function brickWall:look()
	showMessage("It's a wall made of red bricks, what a view.");
end

function brickWall:actionIndLookAt(sentence)
	self:look();
end

return brickWall;
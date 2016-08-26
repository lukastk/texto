roomBase = {};
roomBase.type = "room";
roomBase.identifier = "roomBase";
roomBase.names = {};
roomBase.items = {};
roomBase.visible = true;
roomBase.canSeeInside = true;
roomBase.literalActions = {};
--Locations, used with goNorth and etc.
roomBase.north = "";
roomBase.south = "";
roomBase.east = "";
roomBase.west = "";

function roomBase:init()
end

function roomBase:canAddToInventory(item)
	return true;
end

function roomBase:canTakeFromInventory(item)
	return true;
end

function roomBase:look(sentence)
end

function roomBase:makePlayerGoToDirection(direction)
	if self.items["player"] == nil then
		return;
	end

	if direction ~= "" then
		player:setLocation(direction);
		return true;
	end

	return false; --If the action was succesful or not.
end

function roomBase:goNorth(sentence)
	return self:makePlayerGoToDirection(self.north);
end

function roomBase:goSouth(sentence)
	return self:makePlayerGoToDirection(self.south);
end

function roomBase:goWest(sentence)
	return self:makePlayerGoToDirection(self.west);
end

function roomBase:goEast(sentence)
	return self:makePlayerGoToDirection(self.east);
end
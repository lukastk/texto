player = {};
player.type = "player";
player.identifier = "player";
player.items = {};
player.location = "";
player.insideRoom = "";

--Instead of having a init function, there is gamedir/game/init.lua

function player:setLocation(location)
	local newLocation = getItem(location);
	local oldLocation = getItem(self.location);

	if (newLocation.canAddToInventory(item) and oldLocation.canTakeFromInventory(item)) then
		self.location = location;
		if newLocation.type == "room" then
			self.insideRoom = location;
		end

		oldLocation.items[self.identifier] = nil;
		newLocation.items[self.identifier] = self;
		newLocation:actionIndLookAt(null);

		for _, item in pairs(self.items) do
			item.insideRoom = self.insideRoom;
		end

		return true;
	end

	return false;
end

function player:canAddToInventory(item)
	return true;
end

function player:canTakeFromInventory(item)
	return true;
end

return player;
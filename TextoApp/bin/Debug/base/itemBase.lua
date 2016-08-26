itemBase = {};
itemBase.type = "item";
itemBase.identifier = "itemBase";
itemBase.location = "";
itemBase.insideRoom = "";
itemBase.names = {};
itemBase.items = {};
itemBase.visible = true;
itemBase.canSeeInside = false;
itemBase.literalActions = {};

function itemBase:init()
	--You have to add set the location manually (without setLocation) to avoid trouble.
	getItem(self.location).items[self.identifier] = self
end

function itemBase:getInventoryName()
	return self.names[1];
end

function itemBase:look()
	ShowMessage("You see a " .. self.names[1] .. ".");
	EndMessage();
end

function itemBase:setLocation(location)
	newLocation = getItem(location);
	oldLocation = getItem(self.location);

	if (newLocation.canAddToInventory(item) and oldLocation.canTakeFromInventory(item)) then
		self.location = location;
		if newLocation.type == "room" then
			self.insideRoom = location;
		end

		oldLocation.items[self.identifier] = nil;
		newLocation.items[self.identifier] = self;

		return true;
	end

	return false;
end

function itemBase:canAddToInventory(item)
	return false;
end

function itemBase:canTakeFromInventory(item)
	return false;
end
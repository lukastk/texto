doorBase = inheritClasses(itemBase);
doorBase.locks = {};
doorBase.unlocked = {};
doorBase.leadsTo1 = ""
doorBase.leadsTo2 = ""

function doorBase:isUnlocked()
	return table.getSize(self.locks) == 0;
end

function doorBase:getLeadsTo()
	if player.location == self.leadsTo1 then
		return self.leadsTo2
	elseif player.location == self.leadsTo2 then
		return self.leadsTo1
	end
end
		

function doorBase:unlock(key)
	if not key.isKey then
		showMessage("This isn't a key...");
		return;
	end

	if self.locks[key.identifier] == nil then
		showMessage("The key doesn't fit in the lock.");
	else
		self.locks[key.identifier] = nil;
		self.unlocked[key.identifier] = key;

		if self:isUnlocked() then
			showMessage("The door is unlocked!");
		else
			showMessage("You unlocked one of the locks on the door!");
		end
	end
end

function doorBase:goThrough()
	if self:isUnlocked() then
		return player:setLocation(self:getLeadsTo());
	else
		showMessage("The door is locked.");
		return false;
	end
end

function doorBase:actionUse(sentence)
	self:goThrough();
end

function doorBase:actionIndUseOn(sentence)
	key = getItemByName(sentence.DirObject.Name);

	if key == nil then
		showMessage("What?");
		return;
	end

	self:unlock(key);
end
function table.getSize(t)
	c = 0;
	for k, v in pairs(t) do
		c = c + 1;
	end

	return c;
end

function table.contains(table, element)
  for _, value in pairs(table) do
    if value == element then
      return true
    end
  end
  return false
end


function inheritClasses (...)
	local c = {};

	for i, baseClass in ipairs(arg) do
		for k, v in pairs(baseClass) do
			c[k] = v;
			c["_" .. baseClass.identifier .. "_" .. k] = v;
		end
	end
	
	return c;
end

function makeSynonyms(obj, func, names)
	for i, name in pairs(names) do
		obj[name] = func;
	end
end

function makeDefaultTakeAction(obj, conditionFunc)
	if conditionFunc == nil then
		return function(sentence)
			if player.items[obj.identifier] ~= nil then
				showMessage("You already have the " .. obj.names[1] .. "!");
				return;
			end

			if obj:setLocation(player.identifier) then
				showMessage("You took the " .. obj.names[1] .. ".");
			end
		end
	else
		return function(sentence)
			if not conditionFunc() then
				return;
			end

			if player.items[obj.identifier] ~= nil then
				showMessage("You already have the " .. obj.names[1] .. "!");
				return;
			end

			if obj:setLocation(player.identifier) then
				showMessage("You took the " .. obj.names[1] .. ".");
			end
		end
	end
end

function makeTakeAction(obj, takeMessage, alreadyHaveMessage, conditionFunc)
	if conditionFunc == nil then
		return function(sentence)
			if player.items[obj.identifier] ~= nil then
				showMessage(alreadyHaveMessage);
				return;
			end

			if obj:setLocation(player.identifier) then
				showMessage(takeMessage);
			end
		end
	else
		return function(sentence)
			if not conditionFunc() then
				return;
			end

			if player.items[obj.identifier] ~= nil then
				showMessage(alreadyHaveMessage);
				return;
			end

			if obj:setLocation(player.identifier) then
				showMessage(takeMessage);
			end
		end
	end
end
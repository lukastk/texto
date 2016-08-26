shorthands = {};
shorthands.triggers = {};
shorthands.functions = {};

--TODO: CHECK IN EACH FUNCTION THAT THE ITEM ACTUALLY HAS THE METHOD.

--Custom
shorthands.triggers.look = {"look", "look/"};
function shorthands.functions.look(trigger, keyword, sentence)
	if keyword == "" then
		getItem(player.location):actionIndLookAt(sentence);
	else
		local item = getItemByName(keyword, true);
		if item == nil then
			showMessage("You see no " .. keyword .. ".");
			return;
		end
		getItemByName(keyword):actionIndLookAt(sentence);
	end
end

shorthands.triggers.punch = {"punch/"};
function shorthands.functions.punch(trigger, keyword, sentence)
	showMessage("But i kinda like " .. keyword .. "...");
end

shorthands.triggers.north = {"north", "n"};
function shorthands.functions.north(entry, sentence)
	getItem(player.location):goNorth(sentence);
end

shorthands.triggers.south = {"south", "s"};
function shorthands.functions.south(entry, sentence)
	getItem(player.location):goSouth(sentence);
end

shorthands.triggers.east = {"east", "e"};
function shorthands.functions.east(entry, sentence)
	getItem(player.location):goEast(sentence);
end

shorthands.triggers.west = {"west", "w"};
function shorthands.functions.west(sentence)
	getItem(player.location):goWest(sentence);
end

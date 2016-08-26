guy = inheritClasses(itemBase);
guy.type = "item";
guy.identifier = "blackRoomGuy";
guy.location = "blackRoom";
guy.insideRoom = "blackRoom";
guy.names = {"guy"};
guy.items = {};
guy.visible = true;

function guy:init()
	itemBase.init(self);
end

function guy:look()
	if getItem("blackRoom").isDark then
		showMessage("You can't actually see him through the darkness.");
	else
		showMessage("It's a funny-looking guy with a flashlight.");
	end
end

function guy:actionIndLookAt(sentence)
	self:look();
end

function guy:actionIndTalkTo(sentence)
	showMessage("He says: \"Hi there! It's pretty dark in here right? For some gold i can turn on my flashlight for you!\"");
end

function guy:actionIndGiveTo(sentence)
	item = getItemByName(sentence.DirObject.Name)

	if item.identifier == "whiteRoomGold" then
		showMessage("He says: \"Thanks!, i'll turn on my flashlight.\"");
		getItem("blackRoom").isDark = false;

		item:setLocation("blackRoomGuy");
	else
		showMessage("He says: \"Yeah... I don't want that.\"");
	end
end

return guy;
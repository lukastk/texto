player.location = "whiteRoom";
player.insideRoom = "whiteRoom";
getItem(player.location).items[player.identifier] = player;

showMessage("You wake up to see... white. An extremely white-as-snow kind of white that makes you wish you had some sunglasses. " ..
	"At first you thought that you somehow - against all odds - have ended up in heaven, but when you moved your eyes a little " ..
	"bit you realised that you're actually inside a room.");
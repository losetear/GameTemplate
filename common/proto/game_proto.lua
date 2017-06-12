local sparser = require "sprotoparser"

local game_proto = {}

local types = [[

.package {
	type 0 : integer
	session 1 : integer
}

.general {
	name 0 : string
	race 1 : string
	gameclass 2 : string
	map 3 : string
}

.position {
	x 0 : integer
	y 1 : integer
	z 2 : integer
	o 3 : integer
}

.movement {
	pos 0 : position
}

.movementfull {
	pos 0 : position
	dir 1 : position
	speed 2 : integer
}

.attribute {
	health 0 : integer
	level 1 : integer
	exp 2 : integer
	health_max 3 : integer
	strength 4 : integer
	stamina 5 : integer
	attack_power 6 : integer
}

.attribute_overview {
	level 0 : integer
}

.attribute_aoi {
	level 0 : integer
	health 1 : integer
	health_max 2 : integer
}

.character {
	id 0 : integer
	general 1 : general
	attribute 2 : attribute
	movement 3 : movement
}

.character_agent {
	id 0 : integer
	general 1 : general
	attribute 2 : attribute
	movement 3 : movement
}

.character_overview {
	id 0 : integer
	general 1 : general
	attribute 2 : attribute_overview
}

.character_aoi {
	id 0 : integer
	general 1 : general
	attribute 2 : attribute_aoi
	movement 3 : movement
}

.character_aoi_move {
	id 0 : integer
	movement 1 : movement
}

.character_aoi_attribute {
	id 0 : integer
	attribute 1 : attribute_aoi
}
]]

local c2s = [[
syncclientlag 100 {
	request {
		clienttime 0 : integer
	}
	response {
		clienttime 0 : integer
	}
}

syncserverlag 101 {
	request {
		ping 0 : integer
	}
}

character_list 1000 {
	response {
		character 0 : *character_overview(id)
	}
}

character_create 1001 {
	request {
		character 0 : general
	}

	response {
		character 0 : character_overview
		errno 1 : integer
	}
}

character_pick 1002 {
	request {
		id 0 : integer 
	}

	response {
		character 0 : character
		errno 1 : integer
	}
}

map_ready 1003 {
}

move 1004 {
	request {
		pos 0 : position
	}
	response {
		pos 0 : position
	}
}

combat 1005 {
	request {
		target 0 : integer
	}
	response {
		target 0 : integer
		damage 1 : integer
	}
}


fullmove 1006 {
	request {
		move 0 : movementfull
	}
	response {
		pos 0 : position
	}
}

]]

local s2c = [[
aoi_add 2000 {
	request {
		character 0 : character_aoi
	}
	response {
		wantmore 0 : boolean
	}
}

aoi_remove 2001 {
	request {
		character 0 : integer
	}
}

aoi_update_move 2002 {
	request {
		character 0 : character_aoi_move
	}
	response {
		wantmore 0 : boolean
	}
}

aoi_update_attribute 2003 {
	request {
		character 0 : character_aoi_attribute
	}
	response {
		wantmore 0 : boolean
	}
}

aoi_update_move_full 2004 {
	request {
		movefull 0 : movementfull
	}
	response {
		wantmore 0 : boolean
	}
}

]]

game_proto.types = sparser.parse (types)
game_proto.c2s = sparser.parse (types .. c2s.. s2c)
game_proto.s2c = sparser.parse (types .. s2c)

return game_proto

// Generated by sprotodump. DO NOT EDIT!
// source: game_proto.sproto

using System;
using Sproto;
using System.Collections.Generic;

namespace SprotoType { 
	public class attribute : SprotoTypeBase {
		private static int max_field_count = 7;
		
		
		private Int64 _health; // tag 0
		public Int64 health {
			get { return _health; }
			set { base.has_field.set_field (0, true); _health = value; }
		}
		public bool HasHealth {
			get { return base.has_field.has_field (0); }
		}

		private Int64 _level; // tag 1
		public Int64 level {
			get { return _level; }
			set { base.has_field.set_field (1, true); _level = value; }
		}
		public bool HasLevel {
			get { return base.has_field.has_field (1); }
		}

		private Int64 _exp; // tag 2
		public Int64 exp {
			get { return _exp; }
			set { base.has_field.set_field (2, true); _exp = value; }
		}
		public bool HasExp {
			get { return base.has_field.has_field (2); }
		}

		private Int64 _health_max; // tag 3
		public Int64 health_max {
			get { return _health_max; }
			set { base.has_field.set_field (3, true); _health_max = value; }
		}
		public bool HasHealth_max {
			get { return base.has_field.has_field (3); }
		}

		private Int64 _strength; // tag 4
		public Int64 strength {
			get { return _strength; }
			set { base.has_field.set_field (4, true); _strength = value; }
		}
		public bool HasStrength {
			get { return base.has_field.has_field (4); }
		}

		private Int64 _stamina; // tag 5
		public Int64 stamina {
			get { return _stamina; }
			set { base.has_field.set_field (5, true); _stamina = value; }
		}
		public bool HasStamina {
			get { return base.has_field.has_field (5); }
		}

		private Int64 _attack_power; // tag 6
		public Int64 attack_power {
			get { return _attack_power; }
			set { base.has_field.set_field (6, true); _attack_power = value; }
		}
		public bool HasAttack_power {
			get { return base.has_field.has_field (6); }
		}

		public attribute () : base(max_field_count) {}

		public attribute (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.health = base.deserialize.read_integer ();
					break;
				case 1:
					this.level = base.deserialize.read_integer ();
					break;
				case 2:
					this.exp = base.deserialize.read_integer ();
					break;
				case 3:
					this.health_max = base.deserialize.read_integer ();
					break;
				case 4:
					this.strength = base.deserialize.read_integer ();
					break;
				case 5:
					this.stamina = base.deserialize.read_integer ();
					break;
				case 6:
					this.attack_power = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.health, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_integer (this.level, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_integer (this.exp, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_integer (this.health_max, 3);
			}

			if (base.has_field.has_field (4)) {
				base.serialize.write_integer (this.strength, 4);
			}

			if (base.has_field.has_field (5)) {
				base.serialize.write_integer (this.stamina, 5);
			}

			if (base.has_field.has_field (6)) {
				base.serialize.write_integer (this.attack_power, 6);
			}

			return base.serialize.close ();
		}
	}


	public class attribute_aoi : SprotoTypeBase {
		private static int max_field_count = 3;
		
		
		private Int64 _level; // tag 0
		public Int64 level {
			get { return _level; }
			set { base.has_field.set_field (0, true); _level = value; }
		}
		public bool HasLevel {
			get { return base.has_field.has_field (0); }
		}

		private Int64 _health; // tag 1
		public Int64 health {
			get { return _health; }
			set { base.has_field.set_field (1, true); _health = value; }
		}
		public bool HasHealth {
			get { return base.has_field.has_field (1); }
		}

		private Int64 _health_max; // tag 2
		public Int64 health_max {
			get { return _health_max; }
			set { base.has_field.set_field (2, true); _health_max = value; }
		}
		public bool HasHealth_max {
			get { return base.has_field.has_field (2); }
		}

		public attribute_aoi () : base(max_field_count) {}

		public attribute_aoi (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.level = base.deserialize.read_integer ();
					break;
				case 1:
					this.health = base.deserialize.read_integer ();
					break;
				case 2:
					this.health_max = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.level, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_integer (this.health, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_integer (this.health_max, 2);
			}

			return base.serialize.close ();
		}
	}


	public class attribute_overview : SprotoTypeBase {
		private static int max_field_count = 1;
		
		
		private Int64 _level; // tag 0
		public Int64 level {
			get { return _level; }
			set { base.has_field.set_field (0, true); _level = value; }
		}
		public bool HasLevel {
			get { return base.has_field.has_field (0); }
		}

		public attribute_overview () : base(max_field_count) {}

		public attribute_overview (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.level = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.level, 0);
			}

			return base.serialize.close ();
		}
	}


	public class character : SprotoTypeBase {
		private static int max_field_count = 4;
		
		
		private Int64 _id; // tag 0
		public Int64 id {
			get { return _id; }
			set { base.has_field.set_field (0, true); _id = value; }
		}
		public bool HasId {
			get { return base.has_field.has_field (0); }
		}

		private general _general; // tag 1
		public general general {
			get { return _general; }
			set { base.has_field.set_field (1, true); _general = value; }
		}
		public bool HasGeneral {
			get { return base.has_field.has_field (1); }
		}

		private attribute _attribute; // tag 2
		public attribute attribute {
			get { return _attribute; }
			set { base.has_field.set_field (2, true); _attribute = value; }
		}
		public bool HasAttribute {
			get { return base.has_field.has_field (2); }
		}

		private movement _movement; // tag 3
		public movement movement {
			get { return _movement; }
			set { base.has_field.set_field (3, true); _movement = value; }
		}
		public bool HasMovement {
			get { return base.has_field.has_field (3); }
		}

		public character () : base(max_field_count) {}

		public character (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.id = base.deserialize.read_integer ();
					break;
				case 1:
					this.general = base.deserialize.read_obj<general> ();
					break;
				case 2:
					this.attribute = base.deserialize.read_obj<attribute> ();
					break;
				case 3:
					this.movement = base.deserialize.read_obj<movement> ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.id, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_obj (this.general, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_obj (this.attribute, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_obj (this.movement, 3);
			}

			return base.serialize.close ();
		}
	}


	public class character_agent : SprotoTypeBase {
		private static int max_field_count = 4;
		
		
		private Int64 _id; // tag 0
		public Int64 id {
			get { return _id; }
			set { base.has_field.set_field (0, true); _id = value; }
		}
		public bool HasId {
			get { return base.has_field.has_field (0); }
		}

		private general _general; // tag 1
		public general general {
			get { return _general; }
			set { base.has_field.set_field (1, true); _general = value; }
		}
		public bool HasGeneral {
			get { return base.has_field.has_field (1); }
		}

		private attribute _attribute; // tag 2
		public attribute attribute {
			get { return _attribute; }
			set { base.has_field.set_field (2, true); _attribute = value; }
		}
		public bool HasAttribute {
			get { return base.has_field.has_field (2); }
		}

		private movement _movement; // tag 3
		public movement movement {
			get { return _movement; }
			set { base.has_field.set_field (3, true); _movement = value; }
		}
		public bool HasMovement {
			get { return base.has_field.has_field (3); }
		}

		public character_agent () : base(max_field_count) {}

		public character_agent (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.id = base.deserialize.read_integer ();
					break;
				case 1:
					this.general = base.deserialize.read_obj<general> ();
					break;
				case 2:
					this.attribute = base.deserialize.read_obj<attribute> ();
					break;
				case 3:
					this.movement = base.deserialize.read_obj<movement> ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.id, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_obj (this.general, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_obj (this.attribute, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_obj (this.movement, 3);
			}

			return base.serialize.close ();
		}
	}


	public class character_aoi : SprotoTypeBase {
		private static int max_field_count = 4;
		
		
		private Int64 _id; // tag 0
		public Int64 id {
			get { return _id; }
			set { base.has_field.set_field (0, true); _id = value; }
		}
		public bool HasId {
			get { return base.has_field.has_field (0); }
		}

		private general _general; // tag 1
		public general general {
			get { return _general; }
			set { base.has_field.set_field (1, true); _general = value; }
		}
		public bool HasGeneral {
			get { return base.has_field.has_field (1); }
		}

		private attribute_aoi _attribute; // tag 2
		public attribute_aoi attribute {
			get { return _attribute; }
			set { base.has_field.set_field (2, true); _attribute = value; }
		}
		public bool HasAttribute {
			get { return base.has_field.has_field (2); }
		}

		private movement _movement; // tag 3
		public movement movement {
			get { return _movement; }
			set { base.has_field.set_field (3, true); _movement = value; }
		}
		public bool HasMovement {
			get { return base.has_field.has_field (3); }
		}

		public character_aoi () : base(max_field_count) {}

		public character_aoi (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.id = base.deserialize.read_integer ();
					break;
				case 1:
					this.general = base.deserialize.read_obj<general> ();
					break;
				case 2:
					this.attribute = base.deserialize.read_obj<attribute_aoi> ();
					break;
				case 3:
					this.movement = base.deserialize.read_obj<movement> ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.id, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_obj (this.general, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_obj (this.attribute, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_obj (this.movement, 3);
			}

			return base.serialize.close ();
		}
	}


	public class character_aoi_attribute : SprotoTypeBase {
		private static int max_field_count = 2;
		
		
		private Int64 _id; // tag 0
		public Int64 id {
			get { return _id; }
			set { base.has_field.set_field (0, true); _id = value; }
		}
		public bool HasId {
			get { return base.has_field.has_field (0); }
		}

		private attribute_aoi _attribute; // tag 1
		public attribute_aoi attribute {
			get { return _attribute; }
			set { base.has_field.set_field (1, true); _attribute = value; }
		}
		public bool HasAttribute {
			get { return base.has_field.has_field (1); }
		}

		public character_aoi_attribute () : base(max_field_count) {}

		public character_aoi_attribute (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.id = base.deserialize.read_integer ();
					break;
				case 1:
					this.attribute = base.deserialize.read_obj<attribute_aoi> ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.id, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_obj (this.attribute, 1);
			}

			return base.serialize.close ();
		}
	}


	public class character_aoi_move : SprotoTypeBase {
		private static int max_field_count = 2;
		
		
		private Int64 _id; // tag 0
		public Int64 id {
			get { return _id; }
			set { base.has_field.set_field (0, true); _id = value; }
		}
		public bool HasId {
			get { return base.has_field.has_field (0); }
		}

		private movement _movement; // tag 1
		public movement movement {
			get { return _movement; }
			set { base.has_field.set_field (1, true); _movement = value; }
		}
		public bool HasMovement {
			get { return base.has_field.has_field (1); }
		}

		public character_aoi_move () : base(max_field_count) {}

		public character_aoi_move (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.id = base.deserialize.read_integer ();
					break;
				case 1:
					this.movement = base.deserialize.read_obj<movement> ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.id, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_obj (this.movement, 1);
			}

			return base.serialize.close ();
		}
	}


	public class character_overview : SprotoTypeBase {
		private static int max_field_count = 3;
		
		
		private Int64 _id; // tag 0
		public Int64 id {
			get { return _id; }
			set { base.has_field.set_field (0, true); _id = value; }
		}
		public bool HasId {
			get { return base.has_field.has_field (0); }
		}

		private general _general; // tag 1
		public general general {
			get { return _general; }
			set { base.has_field.set_field (1, true); _general = value; }
		}
		public bool HasGeneral {
			get { return base.has_field.has_field (1); }
		}

		private attribute_overview _attribute; // tag 2
		public attribute_overview attribute {
			get { return _attribute; }
			set { base.has_field.set_field (2, true); _attribute = value; }
		}
		public bool HasAttribute {
			get { return base.has_field.has_field (2); }
		}

		public character_overview () : base(max_field_count) {}

		public character_overview (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.id = base.deserialize.read_integer ();
					break;
				case 1:
					this.general = base.deserialize.read_obj<general> ();
					break;
				case 2:
					this.attribute = base.deserialize.read_obj<attribute_overview> ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.id, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_obj (this.general, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_obj (this.attribute, 2);
			}

			return base.serialize.close ();
		}
	}


	public class general : SprotoTypeBase {
		private static int max_field_count = 4;
		
		
		private string _name; // tag 0
		public string name {
			get { return _name; }
			set { base.has_field.set_field (0, true); _name = value; }
		}
		public bool HasName {
			get { return base.has_field.has_field (0); }
		}

		private string _race; // tag 1
		public string race {
			get { return _race; }
			set { base.has_field.set_field (1, true); _race = value; }
		}
		public bool HasRace {
			get { return base.has_field.has_field (1); }
		}

		private string _gameclass; // tag 2
		public string gameclass {
			get { return _gameclass; }
			set { base.has_field.set_field (2, true); _gameclass = value; }
		}
		public bool HasGameclass {
			get { return base.has_field.has_field (2); }
		}

		private string _map; // tag 3
		public string map {
			get { return _map; }
			set { base.has_field.set_field (3, true); _map = value; }
		}
		public bool HasMap {
			get { return base.has_field.has_field (3); }
		}

		public general () : base(max_field_count) {}

		public general (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.name = base.deserialize.read_string ();
					break;
				case 1:
					this.race = base.deserialize.read_string ();
					break;
				case 2:
					this.gameclass = base.deserialize.read_string ();
					break;
				case 3:
					this.map = base.deserialize.read_string ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_string (this.name, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_string (this.race, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_string (this.gameclass, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_string (this.map, 3);
			}

			return base.serialize.close ();
		}
	}


	public class movement : SprotoTypeBase {
		private static int max_field_count = 1;
		
		
		private position _pos; // tag 0
		public position pos {
			get { return _pos; }
			set { base.has_field.set_field (0, true); _pos = value; }
		}
		public bool HasPos {
			get { return base.has_field.has_field (0); }
		}

		public movement () : base(max_field_count) {}

		public movement (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.pos = base.deserialize.read_obj<position> ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_obj (this.pos, 0);
			}

			return base.serialize.close ();
		}
	}


	public class movementfull : SprotoTypeBase {
		private static int max_field_count = 3;
		
		
		private position _pos; // tag 0
		public position pos {
			get { return _pos; }
			set { base.has_field.set_field (0, true); _pos = value; }
		}
		public bool HasPos {
			get { return base.has_field.has_field (0); }
		}

		private position _dir; // tag 1
		public position dir {
			get { return _dir; }
			set { base.has_field.set_field (1, true); _dir = value; }
		}
		public bool HasDir {
			get { return base.has_field.has_field (1); }
		}

		private Int64 _speed; // tag 2
		public Int64 speed {
			get { return _speed; }
			set { base.has_field.set_field (2, true); _speed = value; }
		}
		public bool HasSpeed {
			get { return base.has_field.has_field (2); }
		}

		public movementfull () : base(max_field_count) {}

		public movementfull (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.pos = base.deserialize.read_obj<position> ();
					break;
				case 1:
					this.dir = base.deserialize.read_obj<position> ();
					break;
				case 2:
					this.speed = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_obj (this.pos, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_obj (this.dir, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_integer (this.speed, 2);
			}

			return base.serialize.close ();
		}
	}


	public class package : SprotoTypeBase {
		private static int max_field_count = 2;
		
		
		private Int64 _type; // tag 0
		public Int64 type {
			get { return _type; }
			set { base.has_field.set_field (0, true); _type = value; }
		}
		public bool HasType {
			get { return base.has_field.has_field (0); }
		}

		private Int64 _session; // tag 1
		public Int64 session {
			get { return _session; }
			set { base.has_field.set_field (1, true); _session = value; }
		}
		public bool HasSession {
			get { return base.has_field.has_field (1); }
		}

		public package () : base(max_field_count) {}

		public package (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.type = base.deserialize.read_integer ();
					break;
				case 1:
					this.session = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.type, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_integer (this.session, 1);
			}

			return base.serialize.close ();
		}
	}


	public class position : SprotoTypeBase {
		private static int max_field_count = 4;
		
		
		private Int64 _x; // tag 0
		public Int64 x {
			get { return _x; }
			set { base.has_field.set_field (0, true); _x = value; }
		}
		public bool HasX {
			get { return base.has_field.has_field (0); }
		}

		private Int64 _y; // tag 1
		public Int64 y {
			get { return _y; }
			set { base.has_field.set_field (1, true); _y = value; }
		}
		public bool HasY {
			get { return base.has_field.has_field (1); }
		}

		private Int64 _z; // tag 2
		public Int64 z {
			get { return _z; }
			set { base.has_field.set_field (2, true); _z = value; }
		}
		public bool HasZ {
			get { return base.has_field.has_field (2); }
		}

		private Int64 _o; // tag 3
		public Int64 o {
			get { return _o; }
			set { base.has_field.set_field (3, true); _o = value; }
		}
		public bool HasO {
			get { return base.has_field.has_field (3); }
		}

		public position () : base(max_field_count) {}

		public position (byte[] buffer) : base(max_field_count, buffer) {
			this.decode ();
		}

		protected override void decode () {
			int tag = -1;
			while (-1 != (tag = base.deserialize.read_tag ())) {
				switch (tag) {
				case 0:
					this.x = base.deserialize.read_integer ();
					break;
				case 1:
					this.y = base.deserialize.read_integer ();
					break;
				case 2:
					this.z = base.deserialize.read_integer ();
					break;
				case 3:
					this.o = base.deserialize.read_integer ();
					break;
				default:
					base.deserialize.read_unknow_data ();
					break;
				}
			}
		}

		public override int encode (SprotoStream stream) {
			base.serialize.open (stream);

			if (base.has_field.has_field (0)) {
				base.serialize.write_integer (this.x, 0);
			}

			if (base.has_field.has_field (1)) {
				base.serialize.write_integer (this.y, 1);
			}

			if (base.has_field.has_field (2)) {
				base.serialize.write_integer (this.z, 2);
			}

			if (base.has_field.has_field (3)) {
				base.serialize.write_integer (this.o, 3);
			}

			return base.serialize.close ();
		}
	}


}


lua sprotodump.lua -cs game_proto.sproto game_protoc2s.sproto game_protos2c.sproto login_protoc2s.sproto -d SprotoCS\
xcopy SprotoCS\*.* ..\..\client\GameTemplateClient\Assets\Scripts\SprotoCS\ /Y
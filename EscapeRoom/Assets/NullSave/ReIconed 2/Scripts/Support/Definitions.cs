﻿namespace NullSave
{
    public static class Definitions
    {

        #region Constants

        public static readonly string[] controllerNames = new string[] { "Unknown Controller", "Steam Controller (non-native)", "Xbox 360 Controller", "Xbox One Controller", "Logitech F310 (D)", "Logitech F310 (X)", "Logitech F710 (D)", "Logitech F710 (X)", "Logitech Dual Action", "Logitech RumblePad 2", "Sony DualShock 2", "Sony DualShock 3", "Sony DualShock 4", "Saitek P880", "Saitek P990", "GGE909 Recoil", "Nyko AirFlo EX", "Radio Shack Gamepad", "GameSir G3 (all models)", "GameSir G4 (all models)", "Zhidong N (XI mode)", "Zhidong N (DI mode)", "Zhidong V+ (XI mode)", "Zhidong V+ (DI mode)", "Zhidong N (Android mode)", "Elecom Gamepad", "N64 Controller (Mayflash adapter)", "GameCube Controller (Mayflash)", "WiiU Pro Controller (Mayflash adapter)", "Atari Jaguar Controller", "Horipad Ultimate (OSX)", "SteelSeries Stratus XL (MFi)", "SteelSeries Nimbus (OSX)", "Mad Catz C.T.R.L.R", "Mad Catz Micro C.T.R.L.R", "Red Samurai Gamepad", "SteelSeries Free", "SteelSeries Stratus XL (Hid)", "idroid Snakebyte (Mode 1)", "idroid Snakebyte (Mode 2)", "ipega Gamepad", "ipega Classic Gamepad", "ipega Multimedia Gamepad", "Nyko Playpad", "Nyko Playpad Pro", "Samsung Gamepad", "Moga Hero Power", "Moga Pro Power", "Thrustmaster Score-A", "Thrustmaster Dual Analog 3.2", "Thrustmaster FireStorm Dual Power", "XiaoMi Gamepad", "Satechi ST-UBGC", "8Bitdo NES30/FC30", "8Bitdo SNES30/SFC30", "8Bitdo NES30/FC30 Pro", "8Bitdo SN30/SF30 Pro", "8Bitdo Zero", "8Bitdo N64", "Buffalo NES Gamepad", "Buffalo SNES Gamepad", "OUYA Gamepad", "GameStick Controller", "Amazon Fire Controller", "Razer Serval", "Nexus Player Gamepad", "Nvidia Shield & Controller (2015)", "Nvidia Shield Controller (2017)", "Sony Playstation Vita", "Nintendo Switch Joy-Con(TM) (L)", "Nintendo Switch Joy-Con(TM) (R)", "Nintendo Switch Joy-Con(TM) (Dual)", "Nintendo Switch Joy-Con(TM) (Handheld)", "Nintendo Switch Pro Controller", "Stadia Controller", "Unity Preconfigured Gamepad", "CH CombatStick", "CH FighterStick", "CH FlightStick Pro", "CH Pro Throttle", "CH Eclipse Yoke", "CH Pro Pedals", "CH Throttle Quadrant", "Saitek X45", "Saitek X52", "Saitek X52 Pro", "Saitek X-55 Rhino Stick", "Saitek X-55 Rhino Throttle", "Saitek X-56 Rhino Stick", "Saitek X-56 Rhino Throttle", "Saitek JI3 Cyborg 3D Gold", "Saitek ST290 Pro", "Saitek Cyborg Evo", "Saitek Pro Flight Yoke", "Saitek Pro Flight Quadrant", "Saitek Pro Flight TPM", "Saitek Pro Flight Cessna Trim Wheel", "Saitek Pro Flight Rudder Pedals", "Mad Catz C.Y.B.O.R.G V1", "Thrustmaster HOTAS Warthog Joystick", "Thrustmaster HOTAS Warthog Throttle", "Thrustmaster T.Flight Hotas X", "Thrustmaster T.Flight Hotas 4", "Thrustmaster T.Flight Stick X", "Thrustmaster T.16000M", "Thrustmaster USB Joystick", "Thrustmaster TWCS Throttle", "Thrustmaster T.Flight Rudder Pedals", "Logitech Extreme 3D Pro", "Logitech WingMan Attack 2", "Logitech Attack 3", "Microsoft SideWinder Joystick", "Microsoft SideWinder 3D Pro", "Microsoft SideWinder FF Pro", "Microsoft SideWinder Precision Pro", "Microsoft SideWinder Precision 2", "VKB-Sim Gladiator MKII", "Pro Flight Trainer PUMA", "Arcade Guns G-500", "Logitech G25", "Logitech G27", "Logitech G29", "Logitech G920", "Logitech Driving Force GT", "Logitech MOMO", "Thrustmaster FFB Wheel", "Thrustmaster F430/T100", "Thrustmaster T150", "Thrustmaster T300 RS", "Thrustmaster T500 RS", "Thrustmaster Ferrari 458 (X360)", "Thrustmaster TH8A Shifter", "Thrustmaster T3PA  Pedals", "Thrustmaster FreeStyler Bike", "Fanatec Porsche 911 Wheel", "XInput Wheel", "XInput Guitar", "VRinsight Ship Console", "RailDriver", "XK-24", "Saitek Heavy Equip Wheel & Pedal", "Saitek Heavy Equip Panel", "3Dconnexion KMJ Emulator", "3Dconnexion SpaceExplorer", "3Dconnexion SpaceMouse", "3Dconnexion SpaceMouse Module", "3Dconnexion SpaceNavigator", "3Dconnexion SpacePilot Pro", "3DRudder", "Oculus Touch Left (Unity)", "Oculus Touch Right (Unity)", "Oculus Remote (Unity)", "Open VR Controller - Left (Unity", "Open VR Controller - Right (Unity)", "Amazon Fire TV Remote", "Nexus Player Remote", "Apple TV Remote (2015)", "Nvidia Shield Remote", "iOS MFi Gamepad", "Sony PS4 Flight Stick", "Sony PS4 Steering Wheel", "Sony PS4 Drums", "Sony PS4 Guitar", "Sony PS VR Aim Controller", "Standardized Gamepad", "Keyboard" };
        public static readonly string[] controllerGuids = new string[] { "00000000-0000-0000-0000-000000000000", "2694f4b9-9d84-4f55-9ee8-78fbba744b7d", "d74a350e-fe8b-4e9e-bbcd-efff16d34115", "19002688-7406-4f4a-8340-8d25335406c8", "027f7200-e0ea-480d-a1be-a51f201d19b9", "d9623ff0-6911-4028-b7a5-b98faa6d2c55", "38648087-ec8f-44f5-b70f-66306a581ebe", "80f1d64b-b462-41cc-8c6d-452e72a2dee6", "886dd9d4-58ce-4212-90f5-2967676e7823", "6242b853-3858-48dc-a89c-d2ac3716e0f0", "c3ad3cad-c7cf-4ca8-8c2e-e3df8d9960bb", "71dfe6c8-9e81-428f-a58e-c7e664b7fbed", "cd9718bf-a87a-44bc-8716-60a0def28a9f", "d57b39e3-3d12-4750-84b0-7a28c5341e88", "34bb2e2c-85c5-47eb-9b77-833c2aac4ec3", "d74cc9ec-a991-4e27-b6c2-b2f50e86f9cf", "ace58550-b1f6-4a3d-8f9b-b9c8331d05c6", "56d68b57-0fb5-4215-8349-82f20f7f854e", "81e23f98-9a7c-4d66-8dcd-65eb0bf151be", "fb783f0c-18d9-4eda-add6-b5f57d9e9918", "7a1d6431-06b0-4fd1-8597-5934d5d7ff63", "f814f772-c01d-4ef6-b037-07ba80df860f", "cc7eb367-06ed-4992-a67b-ee90e83d177b", "3f146646-34da-4f9d-9946-a6e405a8640b", "5fd2d100-cf24-463a-8292-b571955d97e5", "1b32ce5f-55cb-432a-8fed-0c0aff612414", "108946f0-7404-4d3e-a427-6b7f33103f28", "e420e480-00bb-4bd4-b871-95f5f9f1a90d", "470d8395-1fd3-4452-a86a-61cd3c2a6c87", "f1a9b442-f42a-450a-bc75-5e7998658fd3", "9c2ba5e1-d64d-4559-bbd6-09250ab87c9c", "7ab64ae8-6c60-4fb5-850b-2ca9d245c887", "09747016-5765-45a1-ac7b-fbf9c9ec3e2a", "137320b3-9897-4c36-a3d9-761c5c861c0d", "0edfca03-23f0-4065-9435-27159bc75247", "f73916b4-2936-40e3-8964-4180d4eb17ea", "064116a9-00d3-42c6-8875-e671b003dd0e", "47b2af4b-4ed7-47cc-bac1-0ca5f40dd13b", "098e3f17-e7be-445c-8426-5929d538e9b1", "4c97e729-2c5f-41fb-9548-e475c2e857f4", "a3e49a2f-b952-429f-b940-d4c6b5a81298", "560d9f5b-e8cd-4753-996d-782342f723fd", "186e538e-f995-409e-9dac-68a0afa714c4", "d16498f7-43b9-45c5-875a-83840f918e29", "4954a9ce-5a79-4b91-9059-eee8f17ff272", "a97355b3-3c44-4f03-ad96-03a9e804c777", "6cb24ae0-a911-48ea-b11c-36bfeaa54fba", "7e615592-9d24-4aa0-b12d-a854a726f37c", "f8d438bf-b248-4ff6-810c-084db03a5f7f", "8bf3582e-cc1c-4c09-9c69-a8ea04050d16", "03647eb4-aecd-4cf0-a7a4-1f677da5cdc9", "1bcfc2c1-ee2c-4789-b1eb-bc871da34436", "a8481053-229b-4e89-95bc-729069d577c0", "7939b1e7-3422-475c-ba5a-c47b303de2e3", "7939b1e7-3422-475c-ba5a-c47b303de2e4", "84054a74-6b31-4342-9286-67f2ca931127", "6d33fb97-9b0b-4100-a015-668acb50eeeb", "549c2612-815f-4656-a242-815f36e62bf4", "9236c24c-5b45-40fb-b6bc-5b5269fa33dc", "7b244879-85b5-4b91-bcab-81c92f94d818", "b25f3e47-d587-4d7c-8ccb-d834e35213fa", "df6fcab5-d1fb-4797-ace1-580617ddc45e", "47aaa5a4-617e-4650-a38a-1ad2e44f6335", "0d786fc9-84b5-4910-a0fe-b57a9cf7f28a", "4545b6f0-ce10-45be-bdee-52a2b0c1a96c", "5354a895-5a9d-45a2-8800-4f8f5dacb148", "69636ddb-16c2-4272-81a3-d03828644356", "041eed16-8846-4c94-9e38-addd4b9ce00b", "d2aef070-7caa-42ff-b2dc-daac7e4a62b4", "3eb01142-da0e-4a86-8ae8-a15c2b1f2a04", "605dc720-1b38-473d-a459-67d5857aa6ea", "521b808c-0248-4526-bc10-f1d16ee76bf1", "1fbdd13b-0795-4173-8a95-a2a75de9d204", "7bf3154b-9db8-4d52-950f-cd0eed8a5819", "cadc4137-a139-4baa-bcbc-faddb8fd03b1", "28e8a061-b376-4f1d-b6c6-6ca4a406059c", "030d0f5e-b700-461d-81ee-834c9acbfae9", "626d7c4b-f15c-4571-957f-a273e029c17e", "672b88ce-f98b-4535-9603-09d2f4e530ac", "eb43ada3-3513-41a4-9ef1-05348138a1d8", "acd80423-3ce9-4029-9c4b-9230907ea4b1", "3049c96e-9ffd-4088-a136-e92a4d92e2b1", "807fb2c8-9c0e-448e-a364-b80bd08faaec", "da803b47-4e06-4f41-af1a-04db144ecab6", "ec284627-3b35-4731-9c0c-788dc7700743", "d8985c95-8123-41d8-9997-2b42cb2abe38", "0e155f1d-ee21-45b1-b4e6-42bbd3a29a80", "bcb1ba29-5b37-46b4-8029-f1fbe4f1bd85", "d8e96a10-f86a-4364-aaa0-5d99fa017573", "b47b59cb-38e7-4c11-b9bd-b2839d7803f4", "6e9e03d9-b0be-4105-b1cb-f2fa09f56d03", "d508c2d1-4ea2-4bb3-a90d-83164c0f9523", "7ecda8cd-29fb-481e-9ef4-6c243620c899", "6476e035-08d3-4b91-a336-f2e94ad89e6e", "5623cc13-d2b1-4f5e-9b87-d3a1840d3040", "7154dfcd-0926-4228-93c6-ff9a151b8985", "079e4f58-557a-4013-94b6-4c5f97853ad5", "01665f95-8b2d-42d0-8d54-3bc9b0d16b1d", "91f16b9d-5d2f-4c8c-89ab-a0840901cd9a", "ffb9683b-ea6e-4ff6-b0e1-694d33876f72", "6e386ae1-c15b-4bf2-b5ff-0e523a44110e", "d0aae5f0-144b-405f-bf8a-079caac6242a", "ee5daca0-3791-45d2-b979-bbbfdf5085f8", "8d4d6231-4d6d-4f3c-9cbd-89a96c0cdaf1", "f9166a6e-926e-4a12-8abc-d623ff193e3d", "f33c72f3-51a2-4369-9ab3-7a7f077ca790", "f3506772-fad6-4917-b7ef-581bee2159d7", "bdd20bc1-5619-4361-91e0-d652ce750d73", "2a651030-1113-4292-a2e7-d5d37135f720", "01a7ea54-2b95-4e24-bf39-d907195ca476", "e18da850-201b-42bd-b35a-eda7927ae20f", "e4b24a19-53f4-473a-8548-59f6ed442772", "98c83081-e7a8-44a7-9f3e-22e184dd5c98", "c4bf7bca-acf3-4580-a0e2-d68e94bcf638", "017678a7-256a-4b32-a9ae-e5fc1aaf1f53", "be29f7f1-266e-417a-bd70-37d12dccb739", "59462056-360b-4027-b3f8-6e3b050766c9", "258052eb-aa32-4ac1-8c6a-78a353a9a6ba", "1708268b-3914-42c4-a339-7bf4fe83fde2", "12ed988a-b95b-4175-91e5-22b870f4b3f1", "96405233-2f1c-4aa5-a73a-ae2cdf580633", "57379f00-0b2b-4f01-8bc5-5e9bef307350", "0f5ba315-3e83-4237-8374-28bea596914d", "a6b952df-1dee-4b0b-92d2-d124f230e9b1", "f26d10b0-4488-4ed0-8d11-a7d9b71d1d50", "a50d2bcc-7649-462b-9461-74510dbea474", "4ceadb5b-316a-4994-9d98-a644fa515a75", "4d577f61-89a2-4c3f-92eb-e313214907b3", "5a29807c-5915-4af3-8841-cb4c40c15541", "e46e540d-2ae9-4198-91a3-b988efc45b28", "82fe0c2a-80b7-4226-8cda-22b5f6bc6014", "7eca62aa-762d-4c58-bf20-f767320dc7bd", "630123dd-9f69-4cf2-9ad5-79d557318d66", "c87a944a-9bc7-4043-ab22-75ce682c0362", "fe653e2a-aeba-426c-8856-954f37d4d84c", "49d3b7cb-2572-454e-bdd1-ea83e1d744e8", "217c5027-dbf3-4bac-9818-08c9f75467ef", "b72e350d-7180-4397-bcae-2ba898bc3857", "e2095ed1-2809-4c9e-aa2b-c17c8d33b90e", "7780138f-45ca-4632-bd21-733497b44141", "8a78ec94-5674-420e-9e15-abbee68d11ce", "c94c929f-7c09-4d06-8f26-255a58656968", "8afbb479-1c0f-42db-8c21-cefbd3a4c2c8", "dbe24966-c584-4d4e-8d1a-2c70d8b019a7", "151f35f4-43ab-4ae2-b8b9-d0ba9e626684", "fe65a7ec-3cfd-4c68-8b66-6399a3de3b90", "9a06677a-c580-4875-ab68-10c1968478ce", "338893d0-35d9-4ac3-a342-aac32395ddf2", "4c2437d8-438b-400f-bd43-16ba0aadfcc1", "af791365-96f2-4160-9ed6-b0b4f59259d0", "bea8fcd6-2b0e-47d5-b690-ee611d811ed4", "c372a74c-32b5-4926-94e1-b74cc6cf20c3", "9e887770-ca98-4794-b301-6c5ab57c7f8a", "3eaa61f6-1f57-4d16-ab11-af7fc74b5640", "bdd8aa96-cb3c-4c8c-87be-831a28e3c94d", "bd9c3102-0465-4823-a1d1-61089602a217", "bc043dba-df07-4135-929c-5b4398d29579", "891bf3cc-8b1c-4134-9f2f-74330ce430c5", "3d919cfa-468e-49f4-bce9-f6c43f2e7e62", "a75d195b-27a8-41ac-97db-2bd5a649a817", "1b5a521b-6833-4c54-ab6c-bac653c93e9c", "7c338d42-ec21-4402-84ed-7ab547343c19", "274096a0-b4d5-413f-bb4c-7dd68cae6f0f", "65ea105c-6390-4d11-a49b-13a402b1f2d9", "04c23ab3-2b99-4404-a5c4-f0df7e62938f", "Keyboard" };

        #endregion

    }
}
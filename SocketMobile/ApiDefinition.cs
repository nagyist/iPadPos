using System;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;

namespace SocketMobile {



//	[Model]
//	public partial interface ISktScanSymbology {
//
//		[Export ("iD"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanSymbology.h", Line = 14)]
//		[unmapped: unexposed: Elaborated] ID { set; }
//
//		[Export ("getID"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanSymbology.h", Line = 15)]
//		[unmapped: unexposed: Elaborated] GetID { get; }
//
//		[Export ("flags"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanSymbology.h", Line = 16)]
//		[unmapped: unexposed: Elaborated] Flags { set; }
//
//		[Export ("getFlags"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanSymbology.h", Line = 17)]
//		[unmapped: unexposed: Elaborated] GetFlags { get; }
//
//		[Export ("status"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanSymbology.h", Line = 18)]
//		[unmapped: unexposed: Elaborated] Status { set; }
//
//		[Export ("getStatus"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanSymbology.h", Line = 19)]
//		[unmapped: unexposed: Elaborated] GetStatus { get; }
//
//		[Export ("getName"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanSymbology.h", Line = 20)]
//		string GetName { get; }
//	}
//
	[Model, BaseType (typeof (NSObject))]
	public partial interface ISktScanDecodedData {

		[Export ("ID")]
		ESktScanSymbologyID ID { get; }

		[Export ("Name")]
		string Name { get; }

		[Export ("getData")]
		uint GetData { get; }

		[Export ("getDataSize")]
		int GetDataSize { get; }
	}
//
//	[Model]
//	public partial interface ISktScanEvent {
//
//		[Export ("ID"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 13)]
//		[unmapped: unexposed: Elaborated] ID { get; }
//
//		[Export ("getDataType"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 14)]
//		[unmapped: unexposed: Elaborated] GetDataType { get; }
//
//		[Export ("getDataString"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 15)]
//		string GetDataString { get; }
//
//		[Export ("getDataByte"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 16)]
//		uint8_t GetDataByte { get; }
//
//		[Export ("getDataArrayValue"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 17)]
//		[unmapped: pointer: Pointer] GetDataArrayValue { get; }
//
//		[Export ("getDataArraySize"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 18)]
//		uint32_t GetDataArraySize { get; }
//
//		[Export ("getDataLong"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 19)]
//		uint32_t GetDataLong { get; }
//
//		[Export ("getDataDecodedData"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanEvent.h", Line = 20)]
//		ISktScanDecodedData GetDataDecodedData { get; }
//	}
//
//	[Model]
//	public partial interface ISktScanMsg {
//
//		[Export ("MsgID"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanMsg.h", Line = 14)]
//		[unmapped: unexposed: Elaborated] MsgID { get; }
//
//		[Export ("Result"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanMsg.h", Line = 15)]
//		SKTRESULT Result { get; }
//
//		[Export ("DeviceName"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanMsg.h", Line = 16)]
//		string DeviceName { get; }
//
//		[Export ("hDevice"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanMsg.h", Line = 17)]
//		ISktScanDevice HDevice { get; }
//
//		[Export ("DeviceType"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanMsg.h", Line = 18)]
//		uint32_t DeviceType { get; }
//
//		[Export ("DeviceGuid"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanMsg.h", Line = 19)]
//		string DeviceGuid { get; }
//
//		[Export ("Event"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanMsg.h", Line = 20)]
//		ISktScanEvent Event { get; }
//	}
//
//	[Model]
//	public partial interface ISktScanVersion {
//
//		[Export ("getMajor"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 13)]
//		ushort GetMajor { get; }
//
//		[Export ("getMiddle"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 14)]
//		ushort GetMiddle { get; }
//
//		[Export ("getMinor"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 15)]
//		ushort GetMinor { get; }
//
//		[Export ("getBuild"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 16)]
//		uint GetBuild { get; }
//
//		[Export ("getMonth"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 17)]
//		ushort GetMonth { get; }
//
//		[Export ("getDay"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 18)]
//		ushort GetDay { get; }
//
//		[Export ("getYear"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 19)]
//		ushort GetYear { get; }
//
//		[Export ("getHour"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 20)]
//		ushort GetHour { get; }
//
//		[Export ("getMinute"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanVersion.h", Line = 21)]
//		ushort GetMinute { get; }
//	}
//
//	[Model]
//	public partial interface ISktScanProperty {
//
//		[Export ("iD"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 16)]
//		int ID { set; }
//
//		[Export ("getID"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 17)]
//		int GetID { get; }
//
//		[Export ("type"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 19)]
//		[unmapped: unexposed: Elaborated] Type { set; }
//
//		[Export ("getType"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 20)]
//		[unmapped: unexposed: Elaborated] GetType { get; }
//
//		[Export ("byte"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 22)]
//		byte Byte { set; }
//
//		[Export ("getByte"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 23)]
//		byte GetByte { get; }
//
//		[Export ("ulong"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 25)]
//		uint Ulong { set; }
//
//		[Export ("getUlong"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 26)]
//		uint GetUlong { get; }
//
//		[Export ("string"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 28)]
//		string String { set; }
//
//		[Export ("getString"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 29)]
//		string GetString { get; }
//
//		[Export ("setArray:Length:")]
//		void SetArray ([unmapped: pointer: Pointer] values, int length);
//
//		[Export ("getArrayValue"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 32)]
//		[unmapped: pointer: Pointer] GetArrayValue { get; }
//
//		[Export ("getArraySize"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 33)]
//		int GetArraySize { get; }
//
//		[Export ("Version"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 35)]
//		ISktScanVersion Version { get; }
//
//		[Export ("Symbology"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 36)]
//		ISktScanSymbology Symbology { get; }
//
//		[Export ("object"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 38)]
//		NSObject Object { set; }
//
//		[Export ("getContext"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 40)]
//		NSObject GetContext { get; }
//
//		[Export ("context"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanProperty.h", Line = 41)]
//		NSObject Context { set; }
//	}
//
//	[Model]
//	public partial interface ISktScanObject {
//
//		[Export ("Msg"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanObject.h", Line = 14)]
//		ISktScanMsg Msg { get; }
//
//		[Export ("Property"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanObject.h", Line = 15)]
//		ISktScanProperty Property { get; }
//	}
//
//	[Model]
//	public partial interface ISktScanApi : ISktScanDevice {
//
//		[Export ("waitForScanObject:TimeOut:")]
//		SKTRESULT WaitForScanObject (ISktScanObject scanObj, uint ulTimeOut);
//
//		[Export ("releaseScanObject:")]
//		SKTRESULT ReleaseScanObject (ISktScanObject scanObj);
//	}
//
//	[Model]
//	public partial interface ScanAPIObjectDelegate {
//
//		[Export ("startController"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanApi.h", Line = 16)]
//		NSObject StartController { get; }
//	}
//
//	[Model]
//	public partial interface ISktScanDevice {
//
//		[Export ("open:")]
//		SKTRESULT Open (string devicename);
//
//		[Export ("close"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/ISktScanDevice.h", Line = 13)]
//		SKTRESULT Close { get; }
//
//		[Export ("getProperty:")]
//		SKTRESULT GetProperty (ISktScanObject pScanObj);
//
//		[Export ("setProperty:")]
//		SKTRESULT SetProperty (ISktScanObject pScanObj);
//	}
//
//	[BaseType (typeof (NSObject))]
//	public partial interface SktClassFactory {
//
//		[Static, Export ("createScanObject"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/SktClassFactory.h", Line = 16)]
//		ISktScanObject CreateScanObject { get; }
//
//		[Static, Export ("releaseScanObject:")]
//		void ReleaseScanObject (ISktScanObject scanObj);
//
//		[Static, Export ("createScanApiInstance"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/include/SktClassFactory.h", Line = 19)]
//		ISktScanApi CreateScanApiInstance { get; }
//
//		[Static, Export ("releaseScanApiInstance:")]
//		void ReleaseScanApiInstance (ISktScanApi scanApi);
//
//		[Static, Export ("createDeviceInstance:")]
//		ISktScanDevice CreateDeviceInstance (ISktScanApi scanApi);
//
//		[Static, Export ("releaseDeviceInstance:")]
//		void ReleaseDeviceInstance (ISktScanDevice deviceInstance);
//	}


//	[Model]
//	public partial interface Notification {
//
//		[Export ("OnNotify:notificationType:")]
//		void OnNotify (DeviceInfo deviceinfo, [unmapped: unexposed: Elaborated] type);
//	}

//	[BaseType (typeof (NSObject))]
//	public partial interface SymbologyInfo {
//
//		[Export ("initWithSymbology:")]
//		IntPtr Constructor (ISktScanSymbology symbology);
//
//		[Export ("getName"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 39)]
//		string GetName { get; }
//
//		[Export ("name"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 40)]
//		string Name { set; }
//
//		[Export ("getId"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 42)]
//		[unmapped: unexposed: Elaborated] GetId { get; }
//
//		[Export ("id"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 43)]
//		[unmapped: unexposed: Elaborated] Id { set; }
//
//		[Export ("isEnabled"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 45)]
//		bool IsEnabled { get; }
//
//		[Export ("enabled"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 46)]
//		bool Enabled { set; }
//	}
//
	[BaseType (typeof (NSObject))]
	public partial interface DecodedDataInfo {

		[Export ("initWithDecodedData:")]
		IntPtr Constructor (ISktScanDecodedData decodedData);

		[Export ("getSymbologyName")]
		string GetSymbologyName { get; }

		[Export ("symbologyName")]
		string SymbologyName { set; }

		[Export ("getData")]
		uint GetData { get; }

		[Export ("setData:Length:")]
		void SetData (uint data, int length);

		[Export ("getLength")]
		int GetLength { get; }

		[Export ("length")]
		int Length { set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface DeviceInfo {

//		[Export ("init:name:type:")]
//		IntPtr Constructor (ISktScanDevice device, string name, int type);
//
//		[Export ("getSktScanDevice")]
//		ISktScanDevice GetSktScanDevice { get; }

		[Export ("notification")]
		NSObject Notification { set; }

		[Export ("getNotification")]
		NSObject GetNotification { get; }

		[Export ("getName")]
		string GetName { get; }

		[Export ("name")]
		string Name { set; }

		[Export ("getBdAddress")]
		string GetBdAddress { get; }

		[Export ("bdAddress")]
		string BdAddress { set; }

		[Export ("getTypeString")]
		string GetTypeString { get; }

		[Export ("type")]
		int Type { set; }

		[Export ("getFirmwareVersion")]
		string GetFirmwareVersion { get; }

		[Export ("firmwareVersion")]
		string FirmwareVersion { set; }

		[Export ("getBatteryLevel")]
		string GetBatteryLevel { get; }

		[Export ("batteryLevel")]
		string BatteryLevel { set; }

		[Export ("getLocalDecodeAction")]
		int GetLocalDecodeAction { get; }

		[Export ("localDecodeAction")]
		int LocalDecodeAction { set; }

		[Export ("getRumbleSupport")]
		bool GetRumbleSupport { get; }

		[Export ("rumbleSupport")]
		bool RumbleSupport { set; }

		[Export ("getPostamble")]
		string GetPostamble { get; }

		[Export ("postamble")]
		string Postamble { set; }

//		[Export ("decodeData"), Verify ("ObjC method massaged into setter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 123)]
//		ISktScanDecodedData DecodeData { set; }
//
//		[Export ("getDecodedData"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/DeviceInfo.h", Line = 124)]
//		DecodedDataInfo GetDecodedData { get; }
//
//		[Export ("getSymbologyInfo:")]
//		SymbologyInfo GetSymbologyInfo (int index);
//
//		[Export ("addSymbologyInfo:")]
//		void AddSymbologyInfo (ISktScanSymbology symbologyInfo);
//
		[Export ("getSymbologyCount")]
		int GetSymbologyCount { get; }

		[Export ("setPropertyError:Error:")]
		void SetPropertyError (int propertyId, int error);

		[Export ("getPropertyErrorId")]
		int GetPropertyErrorId { get; }

		[Export ("getPropertyError")]
		int GetPropertyError { get; }
	}

//	[Model, BaseType (typeof (NSObject))]
//	public partial interface CommandContextDelegate {
//
//		[Export ("run:")]
//		void  (ISktScanObject scanObj);
//	}
//
//	[BaseType (typeof (NSObject))]
//	public partial interface CommandContext {
//
//		[Export ("retry")]
//		int Retry { get; set; }
//
//		[Export ("status")]
//		[unmapped: unexposed: Elaborated] Status { get; set; }
//
//		[Export ("initWithParam:ScanObj:ScanDevice:Device:Target:Response:")]
//		IntPtr Constructor (bool getOperation, ISktScanObject scanObj, ISktScanDevice scanDevice, DeviceInfo device, NSObject target, Selector response);
//
//		[Export ("dealloc")]
//		void Dealloc ();
//
//		[Export ("getScanDevice"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/ScanApiHelper.h", Line = 60)]
//		ISktScanDevice GetScanDevice { get; }
//
//		[Export ("getScanObject"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/ScanApiHelper.h", Line = 61)]
//		ISktScanObject GetScanObject { get; }
//
//		[Export ("doCallback:")]
//		SKTRESULT DoCallback (ISktScanObject scanObj);
//
//		[Export ("doCommand"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/ScanApiHelper.h", Line = 63)]
//		SKTRESULT DoCommand { get; }
//	}
//


	[Model,Protocol, BaseType (typeof (NSObject))]
	public partial interface ScanApiHelperDelegate {

		[Export ("onDeviceArrival:Device:")]
		void Device (SKTRESULT result, DeviceInfo deviceInfo);

		[Export ("onDeviceRemoval:")]
		void OnDeviceRemoval (DeviceInfo deviceRemoved);

		[Export ("onError:")]
		void OnError (SKTRESULT result);

		[Export ("onDecodedData:DecodedData:")]
		void DecodedData (DeviceInfo device, string decodedData);

		[Export ("onScanApiInitializeComplete:")]
		void OnInitialized (SKTRESULT result);

		[Export ("onScanApiTerminated")]
		void OnScanApiTerminated ();

		[Export ("onErrorRetrievingScanObject:")]
		void OnErrorRetrievingObject (SKTRESULT result);
	}

	[BaseType (typeof (NSObject))]
	public partial interface ScanApiHelper {

		[Export ("delegate")]
		ScanApiHelperDelegate Delegate { set; }

		[Export ("noDeviceText")]
		string NoDeviceText { set; }

		[Export ("getDevicesList")]
		NSDictionary GetDevicesList { get; }

//		[Export ("getDeviceInfoFromScanObject:")]
//		DeviceInfo GetDeviceInfoFromScanObject (ISktScanObject scanObj);

		[Export ("isDeviceConnected")]
		bool IsDeviceConnected { get; }

		[Export ("isScanApiOpen")]
		bool IsScanApiOpen { get; }

		[Export ("open")]
		void Open ();

		[Export ("close")]
		void Close ();

		[Export ("doScanApiReceive")]
		SKTRESULT DoScanApiReceive ();

//		[Export ("removeCommand:")]
//		void RemoveCommand (DeviceInfo deviceInfo);
//
//		[Export ("postGetScanApiVersion:Response:")]
//		void PostGetScanApiVersion (NSObject target, Selector response);
//
//		[Export ("postSetConfirmationMode:Target:Response:")]
//		void PostSetConfirmationMode (byte mode, NSObject target, Selector response);
//
//		[Export ("postScanApiAbort:Response:")]
//		void PostScanApiAbort (NSObject target, Selector response);

		[Export ("postSetDataConfirmation:Target:Response:")]
		void PostSetDataConfirmation (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postSetSoftScanStatus:Target:Response:")]
//		void PostSetSoftScanStatus (byte action, NSObject target, Selector response);
//
//		[Export ("postGetSoftScanStatus:Response:")]
//		void PostGetSoftScanStatus (NSObject target, Selector response);
//
//		[Export ("postGetBtAddress:Target:Response:")]
//		void PostGetBtAddress (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postGetDeviceType:Target:Response:")]
//		void PostGetDeviceType (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postGetFirmwareVersion:Target:Response:")]
//		void PostGetFirmwareVersion (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postGetBattery:Target:Response:")]
//		void PostGetBattery (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postGetDecodeAction:Target:Response:")]
//		void PostGetDecodeAction (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postGetCapabilitiesDevice:Target:Response:")]
//		void PostGetCapabilitiesDevice (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postGetPostambleDevice:Target:Response:")]
//		void PostGetPostambleDevice (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postSetPostambleDevice:Postamble:Target:Response:")]
//		void PostSetPostambleDevice (DeviceInfo deviceInfo, string postamble, NSObject target, Selector response);
//
//		[Export ("postGetSymbologyInfo:SymbologyId:Target:Response:")]
//		void PostGetSymbologyInfo (DeviceInfo deviceInfo, int symbologyId, NSObject target, Selector response);
//
//		[Export ("postSetSymbologyInfo:SymbologyId:Status:Target:Response:")]
//		void PostSetSymbologyInfo (DeviceInfo deviceInfo, int symbologyId, bool status, NSObject target, Selector response);
//
//		[Export ("postGetFriendlyName:Target:Response:")]
//		void PostGetFriendlyName (DeviceInfo deviceInfo, NSObject target, Selector response);
//
//		[Export ("postSetFriendlyName:FriendlyName:Target:Response:")]
//		void PostSetFriendlyName (DeviceInfo deviceInfo, string friendlyName, NSObject target, Selector response);
//
//		[Export ("postSetDecodeAction:DecodeAction:Target:Response:")]
//		void PostSetDecodeAction (DeviceInfo deviceInfo, int decodeAction, NSObject target, Selector response);
//
//		[Export ("postSetOverlayView:OverlayView:Target:Response:")]
//		void PostSetOverlayView (DeviceInfo deviceInfo, NSObject overlayview, NSObject target, Selector response);
//
//		[Export ("postSetTriggerDevice:Action:Target:Response:")]
//		void PostSetTriggerDevice (DeviceInfo deviceInfo, byte action, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingProfiles:Response:")]
//		void PostGetDataEditingProfiles (NSObject target, Selector response);
//
//		[Export ("postSetDataEditingProfiles:Target:Response:")]
//		void PostSetDataEditingProfiles (string profiles, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingCurrentProfile:Response:")]
//		void PostGetDataEditingCurrentProfile (NSObject target, Selector response);
//
//		[Export ("postSetDataEditingCurrentProfile:Target:Response:")]
//		void PostSetDataEditingCurrentProfile (string profile, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingTriggerSymbology:Target:Response:")]
//		void PostGetDataEditingTriggerSymbology (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingTriggerSymbology:Target:Response:")]
//		void PostSetDataEditingTriggerSymbology (string profileAndSymbology, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingTriggerMinLength:Target:Response:")]
//		void PostGetDataEditingTriggerMinLength (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingTriggerMinLength:Target:Response:")]
//		void PostSetDataEditingTriggerMinLength (string profileAndLength, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingTriggerMaxLength:Target:Response:")]
//		void PostGetDataEditingTriggerMaxLength (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingTriggerMaxLength:Target:Response:")]
//		void PostSetDataEditingTriggerMaxLength (string profileAndLength, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingTriggerStartsBy:Target:Response:")]
//		void PostGetDataEditingTriggerStartsBy (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingTriggerStartsBy:Target:Response:")]
//		void PostSetDataEditingTriggerStartsBy (string profileAndString, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingTriggerEndsWith:Target:Response:")]
//		void PostGetDataEditingTriggerEndsWith (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingTriggerEndsWith:Target:Response:")]
//		void PostSetDataEditingTriggerEndsWith (string profileAndString, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingTriggerContains:Target:Response:")]
//		void PostGetDataEditingTriggerContains (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingTriggerContains:Target:Response:")]
//		void PostSetDataEditingTriggerContains (string profileAndString, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingOperations:Target:Response:")]
//		void PostGetDataEditingOperations (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingOperations:Target:Response:")]
//		void PostSetDataEditingOperations (string profileAndOperations, NSObject target, Selector response);
//
//		[Export ("postGetDataEditingExport:Target:Response:")]
//		void PostGetDataEditingExport (string profile, NSObject target, Selector response);
//
//		[Export ("postSetDataEditingImport:Target:Response:")]
//		void PostSetDataEditingImport (string profile, NSObject target, Selector response);

//		[Export ("addCommand:")]
//		void AddCommand (CommandContext command);
//
//		[Export ("initializeScanAPIThread:")]
//		void InitializeScanAPIThread (NSObject arg);
//
//		[Export ("handleScanObject:")]
//		bool HandleScanObject (ISktScanObject scanObj);
//
//		[Export ("handleDeviceArrival:")]
//		SKTRESULT HandleDeviceArrival (ISktScanObject scanObj);
//
//		[Export ("handleDeviceRemoval:")]
//		SKTRESULT HandleDeviceRemoval (ISktScanObject scanObj);
//
//		[Export ("handleSetOrGetComplete:")]
//		SKTRESULT HandleSetOrGetComplete (ISktScanObject scanObj);
//
//		[Export ("handleEvent:")]
//		SKTRESULT HandleEvent (ISktScanObject scanObj);
//
//		[Export ("handleDecodedData:")]
//		SKTRESULT HandleDecodedData (ISktScanObject scanObj);
//
//		[Export ("sendNextCommand"), Verify ("ObjC method massaged into getter property", "/Users/Clancey/Downloads/ScanAPI and Samples/ScanAPI/ScanApiHelper.h", Line = 614)]
//		SKTRESULT SendNextCommand { get; }
	}
}

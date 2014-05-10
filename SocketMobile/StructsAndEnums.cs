using System;

namespace SocketMobile
{
	public enum SKTRESULT {
		ktScanMsgIdNotInitialized,
		ktScanMsgIdDeviceArrival,
		ktScanMsgIdDeviceRemoval,
		ktScanMsgIdTerminate,
		ktScanMsgSetComplete,
		ktScanMsgGetComplete,
		ktScanMsgEvent,
		ktScanMsgLastID
	}

	public enum ESktScanEventID : uint {
		SktScanEventError,
		SktScanEventDecodedData,
		SktScanEventPower,
		SktScanEventButtons,
		SktScanEventBatteryLevel,
		SktScanEventListenerStarted,
		SktScanEventLastID
	}

	public enum ESktEventDataType : uint {
		SktScanEventDataTypeNone,
		SktScanEventDataTypeByte,
		SktScanEventDataTypeUlong,
		SktScanEventDataTypeArray,
		SktScanEventDataTypeString,
		SktScanEventDataTypeDecodedData,
		SktScanEventDataTypeLastID
	}

	public enum ESktScanSymbologyID : uint {
		SktScanSymbologyNotSpecified,
		SktScanSymbologyAustraliaPost,
		SktScanSymbologyAztec,
		SktScanSymbologyBooklandEan,
		SktScanSymbologyBritishPost,
		SktScanSymbologyCanadaPost,
		SktScanSymbologyChinese2of5,
		SktScanSymbologyCodabar,
		SktScanSymbologyCodablockA,
		SktScanSymbologyCodablockF,
		SktScanSymbologyCode11,
		SktScanSymbologyCode39,
		SktScanSymbologyCode39Extended,
		SktScanSymbologyCode39Trioptic,
		SktScanSymbologyCode93,
		SktScanSymbologyCode128,
		SktScanSymbologyDataMatrix,
		SktScanSymbologyDutchPost,
		SktScanSymbologyEan8,
		SktScanSymbologyEan13,
		SktScanSymbologyEan128,
		SktScanSymbologyEan128Irregular,
		SktScanSymbologyEanUccCompositeAB,
		SktScanSymbologyEanUccCompositeC,
		SktScanSymbologyGs1Databar,
		SktScanSymbologyGs1DatabarLimited,
		SktScanSymbologyGs1DatabarExpanded,
		SktScanSymbologyInterleaved2of5,
		SktScanSymbologyIsbt128,
		SktScanSymbologyJapanPost,
		SktScanSymbologyMatrix2of5,
		SktScanSymbologyMaxicode,
		SktScanSymbologyMsi,
		SktScanSymbologyPdf417,
		SktScanSymbologyPdf417Micro,
		SktScanSymbologyPlanet,
		SktScanSymbologyPlessey,
		SktScanSymbologyPostnet,
		SktScanSymbologyQRCode,
		SktScanSymbologyStandard2of5,
		SktScanSymbologyTelepen,
		SktScanSymbologyTlc39,
		SktScanSymbologyUpcA,
		SktScanSymbologyUpcE0,
		SktScanSymbologyUpcE1,
		SktScanSymbologyUspsIntelligentMail,
		SktScanSymbologyDirectPartMarking,
		SktScanSymbologyLastSymbologyID
	}

	public enum ESktScanPropType : uint {
		SktScanPropTypeNone,
		SktScanPropTypeNotApplicable,
		SktScanPropTypeByte,
		SktScanPropTypeUlong,
		SktScanPropTypeArray,
		SktScanPropTypeString,
		SktScanPropTypeVersion,
		SktScanPropTypeSymbology,
		SktScanPropTypeEnum,
		SktScanPropTypeObject,
		SktScanPropTypeLastType
	}

	public enum ESktScanSymbologyFlags : uint {
		SktScanSymbologyFlagStatus = 1,
		SktScanSymbologyFlagParam = 2
	}

	public enum TSktScanArray  {
		SktScanSymbologyStatusDisable = 0,
		SktScanSymbologyStatusEnable = 1,
		SktScanSymbologyStatusNotSupported = 2
	}

	public enum SktScanGroup : uint {
		SktScanGroupGeneral,
		SktScanGroupLocalFunctions
	}

	public enum SktScanId : uint {
		SktScanIdAbort,
		SktScanIdVersion,
		SktScanIdInterfaceVersion,
		SktScanIdConfiguration,
		SktScanIdDataConfirmationMode,
		SktScanIdDataConfirmationAction,
		SktScanIdMonitorMode,
		SktScanIdSoftScanStatus,
		SktScanIdDataEditingProfile,
		SktScanIdDataEditingCurrentProfile,
		SktScanIdDataEditingTriggerSymbologies,
		SktScanIdDataEditingTriggerMinLength,
		SktScanIdDataEditingTriggerMaxLength,
		SktScanIdDataEditingTriggerStartsBy,
		SktScanIdDataEditingTriggerEndsWith,
		SktScanIdDataEditingTriggerContains,
		SktScanIdDataEditingOperation,
		SktScanIdDataEditingImportExport,
		SktScanLastGeneralGroup
	}

	public enum  SktScanIdDevice : uint {
		SktScanIdDeviceVersion,
		SktScanIdDeviceInterfaceVersion,
		SktScanIdDeviceType,
		SktScanIdDeviceSpecific,
		SktScanIdDeviceSymbology,
		SktScanIdDeviceTrigger,
		SktScanIdDeviceApplyConfig,
		SktScanIdDevicePreamble,
		SktScanIdDevicePostamble,
		SktScanIdDeviceCapabilities,
		SktScanIdDeviceChangeId,
		SktScanIdDeviceDataFormat,
		SktScanLastDeviceGeneralGroup
	}


	public enum ESktScanDataConfirmationMode : uint {
		SktScanDataConfirmationModeOff,
		SktScanDataConfirmationModeDevice,
		SktScanDataConfirmationModeScanAPI,
		SktScanDataConfirmationModeApp
	}

	public enum ESktScanDeviceDataAcknowledgment : uint {
		SktScanDeviceDataAcknowledgmentOff,
		SktScanDeviceDataAcknowledgmentOn
	}

	public enum ESktScanSecurityMode : uint {
		SktScanSecurityModeNone,
		SktScanSecurityModeAuthentication,
		SktScanSecurityModeAuthenticationEncryption
	}

	public enum  SktScanTrigger : uint {
		SktScanTriggerStart = 1,
		SktScanTriggerStop,
		SktScanTriggerEnable,
		SktScanTriggerDisable
	}

	public enum SktScanDeletePairing : uint {
		SktScanDeletePairingCurrent = 0,
		SktScanDeletePairingAll = 1
	}

	public enum SktScanSoundActionType : uint {
		SktScanSoundActionTypeGoodScan,
		SktScanSoundActionTypeGoodScanLocal,
		SktScanSoundActionTypeBadScan,
		SktScanSoundActionTypeBadScanLocal
	}

	public enum SktScanSoundFrequency : uint {
		SktScanSoundFrequencyNone = 0,
		SktScanSoundFrequencyLow,
		SktScanSoundFrequencyMedium,
		SktScanSoundFrequencyHigh,
		SktScanSoundFrequencyLast
	}

	public enum  SktScanRumbleActionType : uint {
		SktScanRumbleActionTypeGoodScan,
		SktScanRumbleActionTypeGoodScanLocal,
		SktScanRumbleActionTypeBadScan,
		SktScanRumbleActionTypeBadScanLocal
	}

	public enum SktScanLocalDecodeAction : uint {
		SktScanLocalDecodeActionNone = 0,
		SktScanLocalDecodeActionBeep = 1,
		SktScanLocalDecodeActionFlash = 2,
		SktScanLocalDecodeActionRumble = 4
	}

	public enum SktScanDataConfirmationLed : uint {
		SktScanDataConfirmationLedNone = 0,
		SktScanDataConfirmationLedGreen = 1,
		SktScanDataConfirmationLedRed = 2
	}

	public enum SktScanDataConfirmationBeep : uint {
		SktScanDataConfirmationBeepNone = 0,
		SktScanDataConfirmationBeepGood = 1,
		SktScanDataConfirmationBeepBad = 2
	}

	public enum SktScanDataConfirmationRumble : uint {
		SktScanDataConfirmationRumbleNone = 0,
		SktScanDataConfirmationRumbleGood = 1,
		SktScanDataConfirmationRumbleBad = 2
	}

	public enum SktScanFlash : uint {
		SktScanFlashOff = 0,
		SktScanFlashOn = 1
	}

	public enum  SktScan : uint {
		SktScanEnableSoftScan = 0,
		SktScanDisableSoftScan = 1,
		SktScanSoftScanNotSupported = 2,
		SktScanSoftScanSupported = 3
	}

	public enum  SktScanPowerStatus : uint {
		SktScanPowerStatusUnknown = 0,
		SktScanPowerStatusOnBattery = 1,
		SktScanPowerStatusOnCradle = 2,
		SktScanPowerStatusOnAc = 4
	}

	public enum  SktScanMonitor : uint {
		SktScanMonitorDbgLevel = 1,
		SktScanMonitorDbgChannel,
		SktScanMonitorDbgFileLineLevel,
		SktScanMonitorLast
	}

	public enum  SktScanCapability : uint {
		SktScanCapabilityGeneral = 1,
		SktScanCapabilityLocalFunctions = 2
	}



	public enum SktScanCapabilityLocalFunction : uint {
		SktScanCapabilityLocalFunctionRumble = 1,
		SktScanCapabilityLocalFunctionChangeID = 2
	}

	public enum SktScanCounter : uint {
		SktScanCounterUnknown = 0,
		SktScanCounterConnect = 1,
		SktScanCounterDisconnect = 2,
		SktScanCounterUnbond = 3,
		SktScanCounterFactoryReset = 4,
		SktScanCounterScans = 5,
		SktScanCounterScanButtonUp = 6,
		SktScanCounterScanButtonDown = 7,
		SktScanCounterPowerButtonUp = 8,
		SktScanCounterPowerButtonDown = 9,
		SktScanCounterPowerOnACTimeInMinutes = 10,
		SktScanCounterPowerOnBatTimeInMinutes = 11,
		SktScanCounterRfcommSend = 12,
		SktScanCounterRfcommReceive = 13,
		SktScanCounterRfcommReceiveDiscarded = 14,
		SktScanCounterUartSend = 15,
		SktScanCounterUartReceive = 16,
		SktScanCounterUartReceiveDiscarded = 17,
		SktScanCounterButtonLeftPress = 18,
		SktScanCounterButtonLeftRelease = 19,
		SktScanCounterButtonRightPress = 20,
		SktScanCounterButtonRightRelease = 21,
		SktScanCounterRingUnitDetachEvents = 22,
		SktScanCounterRingUnitAttachEvents = 23,
		SktScanCounterDecodedBytes = 24,
		SktScanCounterAbnormalShutDowns = 25,
		SktScanCounterBatteryChargeCycles = 26,
		SktScanCounterBatteryChangeCount = 27,
		SktScanCounterPowerOn = 28,
		SktScanCounterPowerOff = 29,
		SktScanCounterLast
	}

	public enum SktScanDisconnect : uint {
		SktScanDisconnectStartProfile = 0,
		SktScanDisconnectDisableRadio = 1
	}

	public enum SktScanProfileSelect : uint {
		SktScanProfileSelectNone = 0,
		SktScanProfileSelectSpp = 1,
		SktScanProfileSelectHid = 2
	}

	public enum SktScanProfileConfig : uint {
		SktScanProfileConfigNone = 0,
		SktScanProfileConfigAcceptor = 1,
		SktScanProfileConfigInitiator = 2
	}

	public enum SktScanNotifications : uint {
		SktScanNotificationsScanButtonPress = 1 << 0,
		SktScanNotificationsScanButtonRelease = 1 << 1,
		SktScanNotificationsPowerButtonPress = 1 << 2,
		SktScanNotificationsPowerButtonRelease = 1 << 3,
		SktScanNotificationsPowerState = 1 << 4,
		SktScanNotificationsBatteryLevelChange = 1 << 5
	}

	public enum SktScanTimer : uint {
		SktScanTimerTriggerAutoLockTimeout = 1,
		SktScanTimerPowerOffDisconnected = 2,
		SktScanTimerPowerOffConnected = 4
	}

	public enum SktScanDataFormat : uint {
		SktScanDataFormatRaw = 0,
		SktScanDataFormatPacket = 1
	}

	public enum SktScanTriggerMode : uint {
		SktScanTriggerModeLocalOnly = 1,
		SktScanTriggerModeRemoteAndLocal = 2,
		SktScanTriggerModeAutoLock = 3,
		SktScanTriggerModeNormalLock = 4,
		SktScanTriggerModePresentation = 5
	}

	public enum SktScanConnectReason : uint {
		SktScanConnectReasonUnknown = 0,
		SktScanConnectReasonPowerOn = 1,
		SktScanConnectReasonBarcode = 2,
		SktScanConnectReasonUserAction = 3,
		SktScanConnectReasonHostChange = 4,
		SktScanConnectReasonRetry = 5
	}

	public enum SktScanStartUpRoleSPP : uint {
		Acceptor = 0,
		LastRole = 1
	}

	public enum 	SktScanConnectBeepConfig : uint {
		NoBeep = 0,
		Beep = 1
	}

	public enum  SktScanDeviceTypeInterface : uint {
		None,
		SD,
		CF,
		Bluetooth,
		Serial
	}

	public enum  SktScanDeviceTypeProductId : uint {
		None = 0,
		Id7,
		Id7x,
		Id9,
		Id7xi,
		SoftScan,
		Id8ci,
		Unknown
	}
	public enum ENotificationType : uint {
		NotificationFriendlyName,
		NotificationBluetoothAddress,
		NotificationDeviceType,
		NotificationFirmwareVersion,
		NotificationBattery,
		NotificationLocalDecodeAction,
		NotificationCapabilities,
		NotificationPostamble,
		NotificationSymbology,
		NotificationDecodedData,
		NotificationSetPropertyError
	}
	public enum EStatus : uint {
		StatusReady = 1,
		StatusNotCompleted,
		StatusCompleted
	}
}


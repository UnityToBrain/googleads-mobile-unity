#import <GoogleMobileAds/GoogleMobileAds.h>
#import "GADUUMPDebugSettings.h"
@implementation GADUUMPDebugSettings
- (instancetype)init {
  self = [super init];
  _geography = kGADUUMPDebugGeographyDisabled;
  _testDeviceIdentifiers = [[NSArray alloc] init];
  return self;
}
@end

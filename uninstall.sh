#!/bin/bash

cd `dirname $0`
sfctl application delete --application-id ServiceFabric.DataProtection
sfctl application unprovision --application-type-name ServiceFabric.DataProtectionType --application-type-version 1.0.5
sfctl store delete --content-path ServiceFabric.DataProtection
cd -
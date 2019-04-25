SET ADDRESS= 192.168.1.1

fake.cmd run publish.fsx && echo - Entering publish folder... && cd sample/bin/Release/netcoreapp2.1/linux-arm/publish  && echo - Trying to send to files to raspberry pi && pscp -scp *.* "pi@%ADDRESS%":project/ && echo "Done"
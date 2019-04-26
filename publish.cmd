SET ADDRESS= 192.168.1.103

fake.cmd run publish.fsx && echo  - Trying to send files to raspberry pi && pscp -scp -r sample/bin/Release/netcoreapp2.1/linux-arm/publish "pi@%ADDRESS%":Desktop/ && echo Success
FROM fsharp/fsharp
COPY . .
RUN mono ./.paket/paket.bootstrapper.exe
RUN mono ./.paket/paket.exe restore
RUN mono ./packages/build/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
CMD ["fsharpi", "run.fsx"]

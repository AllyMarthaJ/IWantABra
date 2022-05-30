#!/bin/bash
docker build -t iwantabra -f Dockerfile .
docker save iwantabra --output iwantabra.tar

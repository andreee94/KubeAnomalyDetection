ifeq (,$(shell go env GOBIN))
GOBIN=$(shell go env GOPATH)/bin
else
GOBIN=$(shell go env GOBIN)
endif



help: ## Display this help.
	@awk 'BEGIN {FS = ":.*##"; printf "\nUsage:\n  make \033[36m<target>\033[0m\n"} /^[a-zA-Z_0-9-]+:.*?##/ { printf "  \033[36m%-15s\033[0m %s\n", $$1, $$2 } /^##@/ { printf "\n\033[1m%s\033[0m\n", substr($$0, 5) } ' $(MAKEFILE_LIST)


all: build

fmt: ## Run go fmt against code.
	go fmt ./...

vet: ## Run go vet against code.
	go vet ./...

build: fmt vet ## Build manager binary.
	mkdir -p bin/
	GO111MODULE="on" go build -o bin/kubectl-trainer cmd/kubectl-trainer.go  

run: fmt vet ## Run a controller from your host.
	go run cmd/kubectl-trainer.go

run-new: fmt vet ## Run a controller from your host.
	go run cmd/kubectl-trainer.go new --dry-run

install: build
	sudo cp bin/kubectl-trainer /usr/local/bin/
	
uninstall:
	sudo rm /usr/local/bin/kubectl-trainer

clean:
	go clean
	rm -rf bin/


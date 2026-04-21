package main

import (
	"context"
	"log"
	"os/signal"
	"syscall"

	"github.com/Gujiaweiguo/mi/backend/internal/app"
)

func main() {
	if err := run(); err != nil {
		log.Fatal(err)
	}
}

func run() error {
	ctx, stop := signal.NotifyContext(context.Background(), syscall.SIGINT, syscall.SIGTERM)
	defer stop()

	application, err := app.New()
	if err != nil {
		return err
	}

	if err := application.Run(ctx); err != nil {
		return err
	}

	return nil
}

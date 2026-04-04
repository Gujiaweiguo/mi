package main

import (
	"log"

	"github.com/Gujiaweiguo/mi/backend/internal/app"
)

func main() {
	application, err := app.New()
	if err != nil {
		log.Fatal(err)
	}

	if err := application.Run(); err != nil {
		application.Logger().Sugar().Errorw("server exited", "error", err)
		log.Fatal(err)
	}
}

package main

import (
	"bufio"
	"fmt"
	"io"
	"os"
	"strconv"
	"strings"
)

const inputFilename = "input"

func main() {
	f, err := os.Open(inputFilename)
	if err != nil {
		fmt.Printf("Failed to open '%v': %v", inputFilename, err)
		return
	}
	defer f.Close()
	err = calibrate(f, os.Stdout)
	if err != nil {
		fmt.Printf("Failed: %v", err)
		return
	}
	fmt.Println()
}

func calibrate(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var freq int64
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		i, err := strconv.Atoi(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		freq += int64(i)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	_, err := w.Write([]byte(strconv.FormatInt(freq, 10)))
	if err != nil {
		return fmt.Errorf("write '%v': %v", freq, err)
	}
	return nil
}

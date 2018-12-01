package main

import (
	"bytes"
	"io"
	"strconv"
	"strings"
	"testing"
)

type test struct {
	input  string
	output string
}

var tests = []test{
	test{
		input:  `+1`,
		output: `1`,
	},
	test{
		input: `+1
		-2
		+3
		+1`,
		output: `3`,
	},
	test{
		input: `+1
		+1
		+1`,
		output: `3`,
	},
	test{
		input: `+1
		+1
		-2`,
		output: `0`,
	},
	test{
		input: `-1
		-2
		-3`,
		output: `-6`,
	},
}

func TestCalibrate(t *testing.T) {
	tableTest(calibrate, tests, t)
}

type rwFunc func(io.Reader, io.Writer) error

func tableTest(f rwFunc, tts []test, t *testing.T) {
	for i, tt := range tts {
		t.Run(strconv.Itoa(i+1), func(t *testing.T) {
			tt.run(f, t)
		})
	}
}

func (tt test) run(f rwFunc, t *testing.T) {
	var b bytes.Buffer
	err := f(strings.NewReader(tt.input), &b)
	if err != nil {
		t.Error(err)
	}
	s := b.String()
	if s != tt.output {
		t.Errorf("expected '%v' actual '%v'", tt.output, s)
	}
}

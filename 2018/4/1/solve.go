package main

import (
	"bufio"
	"fmt"
	"io"
	"regexp"
	"sort"
	"strconv"
	"strings"
	"time"
)

func solve(r io.Reader, w io.Writer) error {
	scanner := bufio.NewScanner(r)
	var parser parser
	var events eventsByTime
	for scanner.Scan() {
		s := strings.TrimSpace((scanner.Text()))
		ge, err := parser.event(s)
		if err != nil {
			return fmt.Errorf("parse '%v': %v", s, err)
		}
		events = append(events, ge)
	}
	if err := scanner.Err(); err != nil {
		return fmt.Errorf("read lines: %v", err)
	}
	sort.Sort(events)
	guards := make(map[int]*guard)
	var gs *guardShift
	for _, ge := range events {
		switch ge.event {
		case beginShift:
			if gs != nil {
				g, ok := guards[gs.id]
				if !ok {
					g = &guard{id: gs.id}
					guards[gs.id] = g
				}
				g.shifts = append(g.shifts, *gs)
			}
			gs = &guardShift{id: ge.id}
		case fallAsleep:
			if gs == nil {
				return fmt.Errorf("fall asleep before begin shift")
			}
			gs.sleep[ge.time.Minute()] = true
		case wakeUp:
			if gs == nil {
				return fmt.Errorf("wake up before begin shift")
			}
			gs.wake[ge.time.Minute()] = true
		}
	}
	maxSleepGuardID := -1
	maxSleepTotal := -1
	var maxSleepMinutes [60]int
	for id, g := range guards {
		sleepTotal := 0
		var sleepMinutes [60]int
		for _, gs := range g.shifts {
			state := wakeUp
			for min := 0; min < 60; min++ {
				if gs.wake[min] {
					state = wakeUp
				}
				if gs.sleep[min] {
					state = fallAsleep
				}
				if state == fallAsleep {
					sleepTotal++
					sleepMinutes[min]++
				}
			}
		}
		if sleepTotal > maxSleepTotal {
			maxSleepGuardID = id
			maxSleepTotal = sleepTotal
			maxSleepMinutes = sleepMinutes
		}
	}
	maxSleepMinute := -1
	maxSleepMinuteTotal := -1
	for min, total := range maxSleepMinutes {
		if total > maxSleepMinuteTotal {
			maxSleepMinute = min
			maxSleepMinuteTotal = total
		}
	}
	_, err := fmt.Fprintf(w, "%v", maxSleepGuardID*maxSleepMinute)
	return err
}

type guard struct {
	id     int
	shifts []guardShift
}

type guardShift struct {
	id    int
	wake  [60]bool
	sleep [60]bool
}

const (
	beginShift = iota + 1
	fallAsleep
	wakeUp
)

type guardEvent struct {
	time  time.Time
	id    int
	event int
}

type eventsByTime []guardEvent

func (et eventsByTime) Len() int           { return len(et) }
func (et eventsByTime) Swap(i, j int)      { et[i], et[j] = et[j], et[i] }
func (et eventsByTime) Less(i, j int) bool { return et[i].time.Before(et[j].time) }

type parser struct {
	beginShift *regexp.Regexp
	sleep      *regexp.Regexp
	wake       *regexp.Regexp
}

func (p *parser) event(s string) (guardEvent, error) {
	var ge guardEvent
	var err error
	if err = p.init(); err != nil {
		return ge, fmt.Errorf("init: %v", err)
	}
	const timeLayout = `2006-01-02 15:04`
	if matches := p.beginShift.FindStringSubmatch(s); len(matches) == 3 {
		if ge.time, err = time.Parse(timeLayout, matches[1]); err != nil {
			return ge, fmt.Errorf("parse time '%v': %v", matches[1], err)
		}
		if ge.id, err = strconv.Atoi(matches[2]); err != nil {
			return ge, fmt.Errorf("parse id '%v': %v", matches[2], err)
		}
		ge.event = beginShift
	} else if matches := p.wake.FindStringSubmatch(s); len(matches) == 2 {
		if ge.time, err = time.Parse(timeLayout, matches[1]); err != nil {
			return ge, fmt.Errorf("parse time '%v': %v", matches[1], err)
		}
		ge.event = wakeUp
	} else if matches := p.sleep.FindStringSubmatch(s); len(matches) == 2 {
		if ge.time, err = time.Parse(timeLayout, matches[1]); err != nil {
			return ge, fmt.Errorf("parse time '%v': %v", matches[1], err)
		}
		ge.event = fallAsleep
	} else {
		return ge, fmt.Errorf("no match for '%v'", s)
	}
	return ge, nil
}

func (p *parser) init() error {
	if p == nil {
		return fmt.Errorf("nil p in p.init")
	}
	if p.beginShift != nil {
		return nil
	}
	const shift = `\[(.+)\] Guard #(\d+) begins shift`
	const sleep = `\[(.+)\] falls asleep`
	const wake = `\[(.+)\] wakes up`
	var err error
	if p.beginShift, err = regexp.Compile(shift); err != nil {
		return fmt.Errorf("compile '%v': %v", shift, err)
	}
	if p.sleep, err = regexp.Compile(sleep); err != nil {
		return fmt.Errorf("compile '%v': %v", sleep, err)
	}
	if p.wake, err = regexp.Compile(wake); err != nil {
		return fmt.Errorf("compile '%v': %v", wake, err)
	}
	return nil
}

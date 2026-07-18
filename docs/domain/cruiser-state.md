# Cruiser State

## Evidence scope

This document records the vehicle-state concepts used for Lethal Company v81.
Recheck target-version evidence before changing save or restore behavior.

## Snapshot boundary

A useful cruiser snapshot separates durable vehicle values from live scene
relationships. Position, rotation, velocity, health, and turbo state can be
captured as vehicle state; ship attachment and network application are live
relationships that must be observed again when restoring.

## Restore safety

Restoring a live vehicle can move it into geometry or conflict with the ship
magnet. Do not restore while the cruiser is magneted to the ship. Verify that a
cruiser exists and that a saved snapshot is available before applying state.

## Network application

Server requests and local application are separate stages. A save or load
request is not evidence that the base game has applied the final values. When
validating a restore, observe the relevant local application path after the
networked request.

## Change checklist

1. Keep captured values distinct from live attachment state.
2. Reject restores when the cruiser is absent or magneted to the ship.
3. Treat network requests and local state application as separate events.
4. Validate health and turbo values after the base game applies them.

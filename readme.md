# Susceptible Infected Recovered (SIR) Model Simulator

## Description

Simulation of the SIR model written in C#. Writes per day outputs to CSV for easy ingestion into a visualizer.

## Compartments
- **Susceptible**: The population of people that are suceptible to infection.

- **Infected**: The population of people that are actively infected and infectious.

- **Recovered**: The population of people that have recovered from the infection.

## Population Delta Equations
### New Susceptible Population
	newS = previousState.S + (resusceptibleRate * previousState.R - infectionRate * previousState.I * previousState.S);

- **'resusceptibleRate * previousState.R'**: Rate at which people enter the Susceptible population from the Recovered population due to loss of immunity

- **'\- infectionRate * previousState.I * previousState.S'**: Rate at which people leave the Susceptible population and enter the infected population due to infection

### New Infected Population
	newI = previousState.I + (infectionRate * previousState.I * previousState.S - recoveryRate * previousState.I);

- **'infectionRate * previousState.I * previousState.S'**: Rate at which people enter the Infected population from the Susceptible population due to infection.

- **'\- recoveryRate * previousState.I'**: Rate at which people leave the Infected population and enter the Recovered population due to recovery.

### New Recovered Population
	newR = previousState.R + (recoveryRate * previousState.I - resusceptibleRate * previousState.R);

- **'recoveryRate * previousState.I'**: Rate at which people enter the Recovered population from the Infected population due to recovery.

- **'\- resusceptibleRate * previousState.R'**: Rate at which people leave the Recovered population and enter the Susceptible population due to loss of immunity.

### Values are normalized to force values to sum to ~99% at the end of each iteration. This prevents compounding floating point multiplication errors.
	double sum = newS + newI + newR;
    newS /= sum;
    newI /= sum;
    newR /= sum;

## Usage
	SIR-Model.exe -d 100 -i 0.2 -r .1 -s .05 -S 0.99 -I 0.01 -R 0.0 -o results.csv

## Model Inputs
| Input Parameter | Description | Default Value |
| :--- | :--- | :--- |
| 'days' | Number of days the simulation runs for. | 100 |
| 'infectionRate' | Rate at which susceptible become infected. | 0.0 |
| 'recoveryRate' | Rate at which infected people recover from the infection. | 0.0 |
| 'resusceptibleRate' | Rate at which recovered people lose their immunity and become susceptible. | 0.0 |
| 'initialSusceptible' | Initial susceptible population. | 0.0 |
| 'initialInfected' | Initial infected population. | 0.0 |
| 'initialRecovered' | Initial recovered population. | 0.0 |
| 'outputFile' | Path to the simulation output file. | "output.csv" |

# Model Limitations
- The model does not consider natural birth and death rates within the population.
- It does not account for the mortality rate associated with the disease.
- Similar to the typical SIR model, it assumes a homogeneously mixed population.
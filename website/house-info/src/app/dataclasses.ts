

export class PowerInfo {
    
    currentHour: AccumulatedPowerInfo
    currentDay: AccumulatedPowerInfo
    currentMonth: AccumulatedPowerInfo

    currentDayMax: MaxPowerConsumptionInfo
    currentMonthMax: MaxPowerConsumptionInfo
    currentMonthHighLoadMax: MaxPowerConsumptionInfo
    
    currentAverage: number
    hourPrognosis: number
    currentHourPrice: number
}

export class AccumulatedPowerInfo {
    consumption: number
    cost: number
}
export class MaxPowerConsumptionInfo
{
    consumption: number
    pointInTime: string
}


export class WaterInfo {
    currentHour: number
    today: number
    month: number
    hours: WaterHour[]
}
  
export class WaterHour {
    hour: number
    consumption: number
}

export type Temperatures = Temperature[]

export class Temperature {
  reportTime: string
  sensor: string
  temperature: number
  dayMin: number
  dayMax: number
}


  
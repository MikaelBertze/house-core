

export class PowerInfo {
    currentHour: number
    previousHour: number
    today: number
    todayMax: number
    todayMaxHour: string
    monthMax: number
    monthMaxHour: string
    monthMaxHighLoad: number
    monthMaxHighLoadHour: string
    currentAverage: number
    hourPrognosis: number
    currentHourPrice: number
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


  
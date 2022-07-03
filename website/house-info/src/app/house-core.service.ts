import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PowerInfo, Temperatures, WaterInfo } from './dataclasses';
@Injectable({
  providedIn: 'root'
})


export class HouseCoreService {

  constructor(private httpClient: HttpClient) { }

  public getPowerData() {
    return this.httpClient.get<PowerInfo>("/api/powerinfo")
  }

  public getWaterData() {
    return this.httpClient.get<WaterInfo>("/api/waterinfo")
  }

  public getTemperatureData() {
    return this.httpClient.get<Temperatures>("/api/temperatureinfo")
  }
}

/** 
 * Auto Generated Code
 * Please do not modify this file manually 
 * Assembly Name: "API"
 * Source Type: "C:\Users\13661\OneDrive\文档\GitHub\HousePricePrediction\API\bin\Debug\netcoreapp2.0\API.dll"
 * Generated At: 2018-05-27 18:46:58.847
 */
import { HousePredictionRequest } from '../datatypes/API.Entities.HousePredictionRequest';
import { HousePredictionResponse } from '../datatypes/API.Entities.HousePredictionResponse';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable()
export class PredictionService {
	constructor(private $httpClient: HttpClient) {{}}
	public $baseURL: string = '<API>';
	public Predict(request: HousePredictionRequest): Observable<HousePredictionResponse> {
		return this.$httpClient.post<HousePredictionResponse>(this.$baseURL + 'Prediction/Predict', request, {});
	}
}

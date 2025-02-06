using System;
using System.Diagnostics;

namespace LineRace
{
	public static class Time
	{
		/// <summary>
		/// Часы
		/// </summary>
		private static Stopwatch watch;
		/// <summary>
		/// Прошлое время кадра
		/// </summary>
		private static long previousTicks;
		public static float timing;
		/// <summary>
		/// Количество кадров в секунду
		/// </summary>
		public static float fps;
		private static long previousFPSMeasurementTime;
		private static int counter;
		/// <summary>
		/// Текущее время с запуска приложения
		/// </summary>
		public static float CurrentTime { get; private set; }

		/// <summary>
		/// Разница во времени между кадрами
		/// </summary>
		public static float DeltaTime { get; private set; }

		/// <summary>
		/// Конструктори статического класса
		/// </summary>
		static Time()
		{
			watch = new Stopwatch();
			Reset();
		}

		/// <summary>
		/// Обновление подсчитанных значений
		/// </summary>
		public static void UpdateTime()
		{
			long ticks = watch.Elapsed.Ticks;

			CurrentTime = (float)ticks / TimeSpan.TicksPerSecond;
			DeltaTime = (float)(ticks - previousTicks) / TimeSpan.TicksPerSecond;
			timing = DeltaTime;
			previousTicks = ticks;

			counter++;
			// Если истекла секунда, то обновляем значение FPS и фиксируем момент времени для отсчета следующей секунды
			if (watch.ElapsedMilliseconds - previousFPSMeasurementTime >= 1000)
			{
				fps = counter;
				counter = 0;
				previousFPSMeasurementTime = watch.ElapsedMilliseconds;
			}

		}

		/// <summary>
		/// Сброс таймера и счетчика
		/// </summary>
		public static void Reset()
		{
			watch.Reset();
			fps = 0;
			counter = 0;
			watch.Start();
			previousFPSMeasurementTime = watch.ElapsedMilliseconds;
			previousTicks = watch.Elapsed.Ticks;
		}
	}
}


# KlawiaturaGA - program wykorzystujący algorytm genetyczny do znalezienia najlepszego układu klawiatury
KlawiaturaGA to program wykorzystujący algorytm genetyczny do znalezienia najlepszego układu klawiatury, ze względu na metodę oceny Workmana. Metoda ta ocenia wydajność klawiatury na podstawie liczby zmian rąk oraz liczby przeskoków palców podczas pisania.

Program pozwala na korzystanie z różnych operatorów krzyżowania oraz metod selekcji, co umożliwia łatwe dostosowanie algorytmu do potrzeb użytkownika.

## Działanie

![](https://github.com/PiotrCiba/MSI-KeyboardGA/blob/master/preview.gif)

## Funkcjonalności
KlawiaturaGA oferuje następujące funkcjonalności:
- Metody selekcji:
  - Turniejowa
  - Ruletka
  - Rankingowa

- Operatory krzyżujące:
  - **OX** - Order Crossover
  - **CX** - Cycle Crossover
  - **ERX** - Edge Recombination Crossover
  - **AEX** - Alternating Edge Crossover
  
Program KlawiaturaGA został napisany w języku C#, z wykorzystaniem WPF API dla ułatwienia obsługi oraz wizualizacji wyników. Kod programu został umieszczony na platformie GitHub i jest dostępny pod adresem:

https://github.com/PiotrCiba/KlawiaturaGA

W celu uruchomienia programu, należy pobrać kod źródłowy, skompilować go i uruchomić plik .exe.

﻿using System;
using System.Linq;
using Xamarin.Forms.Maps;
using XamarinTSP.Common;
using XamarinTSP.UI.Abstractions;

namespace XamarinTSP.UI.Models
{
    public class Location : PropertyChangedBase, IDisposable
    {
        public EventHandler OnDispose;

        private Position _position;

        public Guid Id { get; }
        public bool IsSampleData { get; }
        public string Name { get; }
        public string MainDisplayString { get; private set; }
        public string AdditionalLocationInfo { get; private set; }
        public string Coordinates { get; private set; }
        public string Index { get; private set; }

        public Position Position
        {
            get => _position;
            set
            {
                _position = value;
                NotifyOfPropertyChange();
            }
        }
        public Location() { }
        public Location(string name, int index, Address address) : this(index, address)
        {
            Name = name;
            IsSampleData = true;
        }
        public Location(int index, Address address)
        {
            SetIndex(index);
            Id = Guid.NewGuid();
            BuildStrings(address);
            Position = new Position(address.Latitude, address.Longitude);
        }
        public void SetIndex(int index)
        {
            Index = $"{index}.";
        }
        public void Dispose() => OnDispose?.Invoke(this, null);
        private void BuildStrings(Address address)
        {
            Coordinates = $"{address.Latitude } {address.Longitude}";
            var street = $"{address.Thoroughfare} {address.SubThoroughfare}";
            var tmp = new[] {
                    string.Join(" ", street, address.Locality, address.SubLocality),
                    string.Join(" ", address.AdminArea, address.SubAdminArea),
                    string.Join(" ", address.CountryName, address.CountryCode),
                    address.FeatureName,
                    Coordinates
                };
            var retval = tmp.FirstOrDefault(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x));

            MainDisplayString = retval ?? "---";

            AdditionalLocationInfo = string.Join("\n", tmp.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToArray());

            NotifyOfPropertyChange(() => Coordinates);
            NotifyOfPropertyChange(() => MainDisplayString);
            NotifyOfPropertyChange(() => AdditionalLocationInfo);
        }
    }
}

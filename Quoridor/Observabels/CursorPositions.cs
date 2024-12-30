using Microsoft.Extensions.DependencyInjection;
using Quoridor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Observabels
{
    internal class CursorPositions
    {
        public ISubject<int> cx = new BehaviorSubject<int>(0);
        public ISubject<int> cy = new BehaviorSubject<int>(0);
        public Cursor cursor { get; set; }

        public CursorPositions()
        {
            this.cursor = SD.ServiceProvider.GetRequiredService<Cursor>();
            if(this.cursor == null)
            {
                throw new Exception(SD.serviceConfigurationError);
            }

            // Subscribe with multi-line lambda functions
            cx.Subscribe(
                onNext: value =>
                {
                    this.cursor.setCursorPosition(value, (cy is BehaviorSubject<int> b) ? b.Value : 0);
                },
                onError: ex =>
                {

                },
                onCompleted: () =>
                {
                }
            );
            // Subscribe with multi-line lambda functions
            cy.Subscribe(
                onNext: value =>
                {
                    this.cursor.setCursorPosition((cx is BehaviorSubject<int> b) ? b.Value : 0 , value);
                },
                onError: ex =>
                {

                },
                onCompleted: () =>
                {

                }
            );

        }
        public void setCx(int cx)
        {
            this.cx.OnNext(cx);
        }
        public void setCy(int cy)
        {
            this.cy.OnNext(cy);
        }
        public int getCx()
        {
            return ( cx is BehaviorSubject<int> b) ? b.Value : 0 ;
        }
        public int getCy()
        {
            return (cy is BehaviorSubject<int> b) ? b.Value : 0;
        }
    }
}

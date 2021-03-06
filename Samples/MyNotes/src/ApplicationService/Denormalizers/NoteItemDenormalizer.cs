﻿using System.Linq;
using Events;
using Ncqrs.Eventing.ServiceModel.Bus;
using ReadModel;

namespace ApplicationService.Denormalizers
{
    public class NoteItemDenormalizer : IEventHandler<NewNoteAdded>,
                                        IEventHandler<NoteTextChanged>
    {
        public void Handle(NewNoteAdded evnt)
        {
            using (var context = new ReadModelContainer())
            {
                var existing = context.NoteItemSet.SingleOrDefault(x => x.Id == evnt.NoteId);
                if (existing != null)
                {
                    return;                    
                }

                var newItem = new NoteItem
                {
                    Id = evnt.NoteId,
                    Text = evnt.Text,
                    CreationDate = evnt.CreationDate
                };

                context.NoteItemSet.AddObject(newItem);
                context.SaveChanges();
            }
        }

        public void Handle(NoteTextChanged evnt)
        {
            using (var context = new ReadModelContainer())
            {
                var itemToUpdate = context.NoteItemSet.Single(item => item.Id == evnt.NoteId);
                itemToUpdate.Text = evnt.NewText;

                context.SaveChanges();
            }
        }
    }
}
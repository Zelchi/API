using Backend.Server.Config;
using Microsoft.EntityFrameworkCore;

namespace Backend.Server.Routes.Contact;

public class ContactRepository(Database context)
{
    private readonly Database _context = context;

    public async Task<IEnumerable<ContactEntity>> GetAllByAccountIdAsync(int accountId)
    {
        return await _context.Contacts
            .Where(c => c.AccountId == accountId && c.DeletedAt == DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<ContactEntity> GetByIdAsync(int id, int accountId)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && c.AccountId == accountId && c.DeletedAt == DateTime.MinValue);
    }

    public async Task<ContactEntity> CreateAsync(ContactEntity contact)
    {
        contact.CreatedAt = DateTime.UtcNow;
        contact.UpdatedAt = DateTime.UtcNow;
        contact.DeletedAt = DateTime.MinValue;

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<ContactEntity> UpdateAsync(ContactEntity contact)
    {
        contact.UpdatedAt = DateTime.UtcNow;
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task DeleteAsync(int id, int accountId)
    {
        var contact = await GetByIdAsync(id, accountId);
        if (contact != null)
        {
            contact.DeletedAt = DateTime.UtcNow;
            contact.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(contact);
        }
    }

    public async Task<bool> ExistsAsync(int id, int accountId)
    {
        return await _context.Contacts
            .AnyAsync(c => c.Id == id && c.AccountId == accountId && c.DeletedAt == DateTime.MinValue);
    }
}
